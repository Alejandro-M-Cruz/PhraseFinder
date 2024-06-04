using System;
using System.Linq;
using System.Text.RegularExpressions;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class SentenceExtensions
    {
        public static int IndexOfWord(this InfoUnaFrase sentence, int wordIndex)
        {
            var word = sentence.Palabras[wordIndex].Palabra;
            var wordRegex = new Regex($@"\b{word}\b");
            var matches = wordRegex.Matches(sentence.Frase);

            if (matches.Count == 0)
            {
                return -1;
            }

            var previousDuplicates = sentence.Palabras.Take(wordIndex).Count(w => w.Palabra == word);
            return matches[Math.Min(previousDuplicates, matches.Count - 1)].Index;
        }

        public static string SubstringInWordRange(this InfoUnaFrase sentence, int firstWordIndex, int wordCount)
        {
            var startIndex = sentence.IndexOfWord(firstWordIndex);

            if (startIndex == -1)
            {
                return string.Empty;
            }

            var lastWordStartIndex = sentence.IndexOfWord(firstWordIndex + wordCount - 1);

            if (lastWordStartIndex == -1)
            {
                return string.Empty;
            }

            var lastWordEndIndex = lastWordStartIndex + sentence
                .Palabras[firstWordIndex + wordCount - 1].Palabra.Length;
            return sentence.Frase.Substring(startIndex, lastWordEndIndex - startIndex);
        }

    }
}