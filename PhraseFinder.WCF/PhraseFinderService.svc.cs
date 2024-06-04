using System;
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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
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
            try
            {
                _patterns = _phrasePatternService.GetPhrasePatterns().ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error from phrase pattern service: " + e.Message);
            }
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
                Debug.WriteLine("Error from servicio lematizacion: " + e.Message);
            }
            
            var foundPhrases = FindPhrasesInSentences(sentences, paragraphs).ToArray();

            try 
            {
                IncludeDefinitions(ref foundPhrases);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error when including definitions: " + e.Message);
            }

            return new PhraseAnalysis
            {
                ProcessedText = paragraphs.ReconstructText(ParagraphSeparator, SentenceSeparator),
                FoundPhrases = foundPhrases
            };
        }

        private IEnumerable<FoundPhrase> FindPhrasesInSentences(
            IReadOnlyCollection<InfoUnaFrase> sentences, IReadOnlyCollection<Paragraph> paragraphs)
        {
            if (_patterns.Length == 0 || sentences.Count == 0 || paragraphs.Count == 0)
            {
                yield break;
            }

            var paragraphSentenceCounts = paragraphs.Select(p => p.GetSentences().Length).ToArray();

            foreach (var pattern in _patterns)
            {
                var sentenceIndex = 0;
                var currentSentenceInParagraph = 0;
                var currentParagraph = 0;

                foreach (var sentence in sentences)
                {
                    var foundPhrase = pattern.FindPhrase(sentence, sentenceIndex);

                    if (foundPhrase != null)
                    {
                        yield return foundPhrase;
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
            }
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
