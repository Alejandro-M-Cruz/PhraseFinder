using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class WordExtensions
    {
        public static bool IsVerb(this InfoUnaPalabra word)
        {
            return 3000 < word.IdCategoria && word.IdCategoria < 3100;
        }

        public static bool IsPunctuationMark(this InfoUnaPalabra word)
        {
            return word.IdCategoria == 20;
        }

        private static readonly string[] VerbSuffixes = { "se", "te", "me", "os", "nos" };

        public static bool SameWordAs(this InfoUnaPalabra word, string otherWord)
        {
            if (word.Palabra.EqualsIgnoreCase(otherWord))
            {
                return true;
            }

            if (!word.IsVerb())
            {
                return false;
            }

            return word.FormaCanonica.EqualsIgnoreCase(otherWord) || VerbSuffixes.Any(suffix =>
                otherWord.EndsWith(suffix) && 
                otherWord.Substring(0, otherWord.Length - suffix.Length)
                    .EqualsIgnoreCase(word.FormaCanonica));
        }

        public static bool SameWordAnyInflection(this InfoUnaPalabra word, string otherWord)
        {
            return word.Palabra.EqualsIgnoreCase(otherWord) || 
                   word.FormaCanonica.EqualsIgnoreCase(otherWord);
        }
    }
}