using System.Linq;
using PhraseFinder.WCF.Models;
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

        public static bool IsPunctuationMark(this InfoUnaPalabra word)
        {
            return word.IdCategoria == 20;
        }

        public static bool SameWordAs(this InfoUnaPalabra word, string otherWord)
        {
            var phraseTag = otherWord.GetTag();

            if (phraseTag?.Category == PhraseTagCategory.Verb)
            {
                return phraseTag.Value.EqualsIgnoreCase(word.Palabra) ||
                       phraseTag.Value.EqualsIgnoreCase(word.FormaCanonica);
            }

            if (!word.IsVerb())
            {
                return word.Palabra.EqualsIgnoreCase(otherWord);
            }

            if (word.FormaCanonica.EqualsIgnoreCase(otherWord))
            {
                return true;
            }

            return VerbSuffixes.Any(suffix =>
                otherWord.EndsWith(suffix) && 
                otherWord.Substring(0, otherWord.Length - suffix.Length)
                    .EqualsIgnoreCase(word.FormaCanonica));
        }
    }
}