using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Data;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.Models;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF
{
    public class PhraseFinderService : IPhraseFinderService
    {
        private static readonly IEnumerable<Phrase> Phrases;
        private static readonly ServicioLematizacionClient ServicioLematizacion = 
            new ServicioLematizacionClient("BasicHttpsBinding_IServicioLematizacion");

        static PhraseFinderService()
        {
            using (var phrasesService = new PhrasesService())
            {
                Phrases = phrasesService.GetPhrases();
            }
        }

        public async Task<PhraseAnalysis> FindPhrasesAsync(string text)
        {
            var sentences = text.GetSentences().ToList();
            List<InfoUnaFrase> processedSentences;
            FoundPhrase[] foundPhrases;

            try
            {
                processedSentences = await ServicioLematizacion
                    .NuevoReconocerFrasesAsync(sentences, idioma: "es", multiPref: false);
            }
            catch (Exception e)
            {
                return new PhraseAnalysis
                {
                    ProcessedText = string.Join(" ", sentences.Select(s => s.Frase)) + 
                                    $"(error en servicio de lematizacion: {e.Message})"
                };
            }

            try
            {
                foundPhrases = FindPhrasesInSentences(processedSentences).ToArray();
                IncludeDefinitions(ref foundPhrases);
                return new PhraseAnalysis
                {
                    ProcessedText = string.Join(" ", processedSentences.Select(s => s.Frase)),
                    FoundPhrases = foundPhrases
                };
            }
            catch (Exception e)
            {
                return new PhraseAnalysis
                {
                    ProcessedText = string.Join(" ", sentences.Select(s => s.Frase)) + 
                                    $"(error en detección: {e.Message})"
                };
            }
	    }

        private IEnumerable<FoundPhrase> FindPhrasesInSentences(IReadOnlyList<InfoUnaFrase> sentences)
        {
            foreach (var phrase in Phrases)
            {
                var phraseWords = phrase.Pattern.Split(' ').Select(w => w.TrimEnd(',')).ToArray();
                var firstWordInPhrase = phraseWords.First();
                var sentenceIndex = 0;
                var anyWords = 0;

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

                            var phraseWord = phraseWords[phraseWordIndex];

                            if (phraseWord.GetTag(phrase)?.Category == PhraseTagCategory.PlaceholderWord)
                            {
                                anyWords += 3;
                                continue;
                            }

                            if (!w.CoincidesWith(phraseWord))
                            {
                                if (anyWords > 0)
                                {
                                    anyWords--;
                                    phraseWordIndex--;
                                    continue;
                                }

                                isMatch = false;
                                break;
                            }
                        }

                        if (!isMatch)
                        {
                            continue;
                        }

                        var phraseMatch = sentence.SubSentenceInWordRange(i, wordIndex - i);

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
                    sentenceIndex += sentence.Frase.Length + 1;
                }
            }
        }

        private static void IncludeDefinitions(ref FoundPhrase[] foundPhrases)
        {
            using (var phrasesService = new PhrasesService())
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
