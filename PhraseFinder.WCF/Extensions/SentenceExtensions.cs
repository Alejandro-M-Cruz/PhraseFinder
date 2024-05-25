using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class SentenceExtensions
    {
        public static int IndexOfWordInIndex(this InfoUnaFrase sentence, int wordIndex)
        {
            return sentence.Palabras
                .Take(wordIndex)
                .Sum(w => string.IsNullOrEmpty(w.PosMark) ? w.Palabra.Length + 1 : w.Palabra.Length);
        }
    }
}