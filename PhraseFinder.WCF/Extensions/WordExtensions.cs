using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class WordExtensions
    {
        private static readonly string[] VerbSuffixes = { "se", "te", "me", "os", "nos" };

        public static bool IsVerb(this InfoUnaPalabra word)
        {
            return 3000 < word.IdCategoria && word.IdCategoria < 3100;
        }

        public static bool CoincidesWith(this InfoUnaPalabra word, string otherWord)
        {
            if (!word.IsVerb())
            {
                return word.Palabra == otherWord;
            }

            if (otherWord == word.FormaCanonica)
            {
                return true;
            }

            return VerbSuffixes.Any(suffix =>
                otherWord.EndsWith(suffix) && 
                otherWord.Substring(0, otherWord.Length - suffix.Length) == word.FormaCanonica);
        }
    }
}