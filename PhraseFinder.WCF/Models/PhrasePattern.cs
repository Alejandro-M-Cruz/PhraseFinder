using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.ServicioLematizacion;
using System;
using System.Linq;

namespace PhraseFinder.WCF.Models
{
    public class PhrasePattern
    {
        public string Phrase { get; set; }
        public string Pattern { get; set; }
        public string BaseWord { get; set; }
        public int PhraseId { get; set; }

        private static readonly string[] PlaceholderWords = { "algo", "alguien" };
        private static readonly string[] CliticPronouns = { "se", "te", "me", "os", "nos" };

        public FoundPhrase FindPhrase(InfoUnaFrase sentence, int sentenceIndexInText)
        {
            if (sentence.Palabras.Count == 0)
            {
                return null;
            }

            var anyWords = 0;
            var patternWords = Pattern
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.TrimEnd(','))
                .ToArray();

            if (patternWords.Length == 0)
            {
                return null;
            }

            var firstWordInPattern = patternWords[0];

            for (var i = 0; i < sentence.Palabras.Count; i++)
            {
                var word = sentence.Palabras[i];

                if (!word.SameWordAs(firstWordInPattern))
                {
                    continue;
                }

                var wordIndex = i + 1;
                var isMatch = true;

                for (var phraseWordIndex = 1; phraseWordIndex < patternWords.Length; phraseWordIndex++)
                {
                    var phraseWord = patternWords[phraseWordIndex];

                    if (phraseWord.GetTag(this)?.Category == PhraseTagCategory.PlaceholderWord)
                    {
                        if (phraseWordIndex == patternWords.Length - 1)
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

                    if (w.SameWordAs(phraseWord))
                    {
                        continue;
                    }

                    if (w.FormaCanonica == "haber" || CliticPronouns.Any(p => p.EqualsIgnoreCase(w.Palabra)))
                    {
                        if (wordIndex < sentence.Palabras.Count && 
                            sentence.Palabras[wordIndex].SameWordAs(phraseWord))
                        {
                            wordIndex++;
                            continue;
                        }
                    }

                    if (anyWords < 1 || phraseWordIndex == patternWords.Length - 1)
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

                return new FoundPhrase
                {
                    PhraseId = PhraseId,
                    Phrase = Phrase,
                    BaseWord = BaseWord,
                    StartIndex = sentenceIndexInText + sentence.IndexOfWord(i),
                    Match = phraseMatch,
                    Length = phraseMatch.Length
                };
            }

            return null;
        }
    }
}