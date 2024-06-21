using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Data;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.Models;
using PhraseFinder.WCF.ServicioLematizacion;
using ProcesarTextos;

namespace PhraseFinder.WCF
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PhraseFinderService : IPhraseFinderService, IDisposable
    {
        private readonly ServicioLematizacionClient _servicioLematizacion;
        private readonly PhrasePatternService _phrasePatternService;
        private readonly PhrasePattern[] _patterns;
        private const string SentenceSeparator = " ";
        private static readonly string ParagraphSeparator = Environment.NewLine + Environment.NewLine;

        public PhraseFinderService()
        {
            _servicioLematizacion = new ServicioLematizacionClient(
                "BasicHttpsBinding_IServicioLematizacion");
            _phrasePatternService = new PhrasePatternService();
            _patterns = _phrasePatternService.GetPhrasePatterns().ToArray();
        }

        public async Task<PhraseAnalysis> FindPhrasesAsync(string text)
        {
            var paragraphs = text.GetParagraphs();
            var sentences = paragraphs.SelectAllSentences().ToList();
            try
            {
                sentences = await _servicioLematizacion
                    .NuevoReconocerFrasesAsync(sentences, idioma: "es", multiPref: false);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error en ServicioLematizacion: {e.Message}");
            }
            
            var foundPhrases = FindPhrasesInSentencesParallel(sentences, paragraphs)
                .Distinct()
                .ToArray();
            IncludeDefinitions(ref foundPhrases);

            return new PhraseAnalysis
            {
                ProcessedText = paragraphs.ReconstructText(ParagraphSeparator, SentenceSeparator),
                FoundPhrases = foundPhrases
            };
        }

        private IEnumerable<FoundPhrase> FindPhrasesInSentencesParallel(
            IReadOnlyCollection<InfoUnaFrase> sentences, 
            IReadOnlyCollection<Paragraph> paragraphs)
        {
            if (_patterns.Length == 0 || sentences.Count == 0 || paragraphs.Count == 0)
            {
                return Enumerable.Empty<FoundPhrase>();
            }

            var paragraphSentenceCounts = paragraphs.Select(p => p.GetSentences().Length).ToArray();

            var foundPhrases = new ConcurrentBag<FoundPhrase>();

            Parallel.ForEach(_patterns, pattern =>
            {
                var sentenceIndex = 0;
                var currentSentenceInParagraph = 0;
                var currentParagraph = 0;

                foreach (var sentence in sentences)
                {
                    var matches = pattern.FindPhrase(sentence, sentenceIndex);

                    foreach (var match in matches)
                    {
                        foundPhrases.Add(match);
                    }

                    sentenceIndex += sentence.Frase.Length;

                    if (currentParagraph < paragraphs.Count &&
                        currentSentenceInParagraph >= paragraphSentenceCounts[currentParagraph] - 1)
                    {
                        sentenceIndex += ParagraphSeparator.Length;
                        currentParagraph++;
                        currentSentenceInParagraph = 0;
                    }
                    else
                    {
                        sentenceIndex += SentenceSeparator.Length;
                        currentSentenceInParagraph++;
                    }
                }
            });

            return foundPhrases.AsEnumerable();
        }

        private void IncludeDefinitions(ref FoundPhrase[] foundPhrases)
        {
            _phrasePatternService.LoadPhraseDefinitions(
                foundPhrases.Select(fp => fp.PhraseId).ToArray());

            foreach (var foundPhrase in foundPhrases)
            {
                foundPhrase.Definitions = _phrasePatternService.GetPhraseDefinitions(foundPhrase.PhraseId);
            }
        }

        public void Dispose()
        {
            _phrasePatternService.Dispose();
        }
    }
}
