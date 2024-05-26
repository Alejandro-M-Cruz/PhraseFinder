using System.Collections.Generic;
using System.Linq;
using PhraseFinder.WCF.Models;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] PlaceholderWords = { "algo", "alguien" };

        public static IEnumerable<InfoUnaFrase> GetSentences(this string text)
        {
            var textProcessor = new ProcesarTextos.Text(string.Empty, text);
            var sentences = textProcessor
                .GetParagraphs()
                .SelectMany(p => p.GetSentences());

            return sentences.Select(s => new InfoUnaFrase
            {
                Frase = s.getText(),
                Palabras = s.GetWords().Select(w => new InfoUnaPalabra()
                {
                    Palabra = w.ToString()
                }).ToList()
            });
        }

        public static bool EqualsIgnoreCase(this string str, string other)
        {
            return str.Equals(other, System.StringComparison.OrdinalIgnoreCase);
        }

        public static PhraseTag GetTag(this string str, Phrase phrase = null)
        {
            if (str.Length < 3)
            {
                return null;
            }

            if (phrase != null && str.IsPlaceholderWord(phrase))
            {
                return new PhraseTag
                {
                    Category = PhraseTagCategory.PlaceholderWord,
                    Value = str
                };
            }

            if (!str.StartsWith("<") || !str.EndsWith(">"))
            {
                return null;
            }

            if (str[1] == '$')
            {
                return new PhraseTag
                {
                    Category = PhraseTagCategory.Verb,
                    Value = str.Substring(2, str.Length - 3)
                };
            }

            return null;
        }

        public static bool IsPlaceholderWord(this string word, Phrase phrase)
        {
            return PlaceholderWords.Any(w => word == w && phrase.BaseWord != w) &&
                   phrase.Value.Split(' ').Length > 2;
        }
    }
}