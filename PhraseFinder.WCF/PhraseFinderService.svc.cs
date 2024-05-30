using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Data;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.Models;
using PhraseFinder.WCF.ServicioLematizacion;
using ProcesarTextos;

namespace PhraseFinder.WCF
{
    public class PhraseFinderService : IPhraseFinderService
    {
        private static readonly ServicioLematizacionClient ServicioLematizacion = 
            new ServicioLematizacionClient("BasicHttpsBinding_IServicioLematizacion");
        private static readonly PhrasePattern[] Phrases;
        private const string SentenceSeparator = " ";
        private static readonly string ParagraphSeparator = Environment.NewLine + Environment.NewLine;

        static PhraseFinderService()
        {
            using (var phrasesService = new PhrasePatternService())
            {
                Phrases = phrasesService.GetPhrasePatterns().ToArray();
            }
        }

        public async Task<PhraseAnalysis> FindPhrasesAsync(string text)
        {
            var paragraphs = text.GetParagraphs();
            var sentences = paragraphs.SelectAllSentences().ToList();

            try
            {
                sentences = await ServicioLematizacion
                    .NuevoReconocerFrasesAsync(sentences, idioma: "es", multiPref: false);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error from servicio lematizacion: " + e.Message);
            }
            
            var foundPhrases = FindPhrasesInSentences(sentences, paragraphs).ToArray();

            IncludeDefinitions(ref foundPhrases);

            return new PhraseAnalysis
            {
                ProcessedText = paragraphs.ReconstructText(ParagraphSeparator, SentenceSeparator),
                FoundPhrases = foundPhrases
            };
        }

        private static IEnumerable<FoundPhrase> FindPhrasesInSentences(
            IReadOnlyCollection<InfoUnaFrase> sentences, IReadOnlyCollection<Paragraph> paragraphs)
        {
            if (Phrases.Length == 0 || sentences.Count == 0 || paragraphs.Count == 0)
            {
                yield break;
            }

            var paragraphSentenceCounts = paragraphs.Select(p => p.GetSentences().Length).ToArray();

            foreach (var phrase in Phrases)
            {
                var phraseWords = phrase.Pattern.Split(' ').Select(w => w.TrimEnd(',')).ToArray();
                var firstWordInPhrase = phraseWords.First();
                var sentenceIndex = 0;
                var anyWords = 0;
                var currentSentenceInParagraph = 0;
                var currentParagraph = 0;

                foreach (var sentence in sentences)
                {
                    for (var i = 0; i < sentence.Palabras.Count; i++)
                    {
                        var word = sentence.Palabras[i];

                        if (!word.CoincidesWith(firstWordInPhrase))
                        {
                            continue;
                        }

                        var wordIndex = i + 1;
                        var isMatch = true;

                        for (var phraseWordIndex = 1; phraseWordIndex < phraseWords.Length; phraseWordIndex++)
                        {
                            var phraseWord = phraseWords[phraseWordIndex];

                            if (phraseWord.GetTag(phrase)?.Category == PhraseTagCategory.PlaceholderWord)
                            {
                                if (phraseWordIndex == phraseWords.Length - 1)
                                {
                                    break;
                                }

                                wordIndex = Math.Max(0, wordIndex - 1);
                                anyWords += 3;
                                continue;
                            }

                            if (wordIndex >= sentence.Palabras.Count)
                            {
                                isMatch = false;
                                break;
                            }

                            var w = sentence.Palabras[wordIndex++];

                            if (w.IsPunctuationMark())
                            {
                                phraseWordIndex--;
                                continue;
                            }

                            if (w.CoincidesWith(phraseWord))
                            {
                                continue;
                            }

                            if (phraseWordIndex == phraseWords.Length - 1 || anyWords < 1)
                            {
                                isMatch = false;
                                break;
                            }

                            anyWords--;
                            phraseWordIndex--;
                        }

                        if (!isMatch)
                        {
                            continue;
                        }

                        var phraseMatch = sentence.SubstringInWordRange(i, wordIndex - i);

                        yield return new FoundPhrase
                        {
                            PhraseId = phrase.PhraseId,
                            Phrase = phrase.Value,
                            BaseWord = phrase.BaseWord,
                            StartIndex = sentenceIndex + sentence.StartIndexOfWord(i),
                            Match = phraseMatch,
                            Length = phraseMatch.Length
                        };
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

        private static void IncludeDefinitions(ref FoundPhrase[] foundPhrases)
        {
            using (var phrasesService = new PhrasePatternService())
            {
                phrasesService.LoadPhraseDefinitions(foundPhrases.Select(fp => fp.PhraseId).ToArray());

                foreach (var foundPhrase in foundPhrases)
                {
                    foundPhrase.Definitions = phrasesService.GetPhraseDefinitions(foundPhrase.PhraseId);
                }
            }
        }
    }
}
