using System.Collections.Generic;
using System.Linq;
using PhraseFinder.WCF.Models;
using PhraseFinder.WCF.ServicioLematizacion;
using ProcesarTextos;

namespace PhraseFinder.WCF.Extensions
{
    public static class StringExtensions
    {
        private static readonly string[] PlaceholderWords = { "algo", "alguien" };

        public static Paragraph[] GetParagraphs(this string text)
        {
            var textProcessor = new ProcesarTextos.Text(string.Empty, text);
            return textProcessor.GetParagraphs();
        }
        
        public static bool EqualsIgnoreCase(this string str, string other)
        {
            return str.Equals(other, System.StringComparison.OrdinalIgnoreCase);
        }

        public static PhraseTag GetTag(this string str, PhrasePattern phrasePattern = null)
        {
            if (str.Length < 3)
            {
                return null;
            }

            if (phrasePattern != null && str.IsPlaceholderWord(phrasePattern))
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

        public static bool IsPlaceholderWord(this string word, PhrasePattern phrasePattern)
        {
            return PlaceholderWords.Any(w => word == w && phrasePattern.BaseWord != w) &&
                   phrasePattern.Value.Split(' ').Length > 2;
        }
    }
}