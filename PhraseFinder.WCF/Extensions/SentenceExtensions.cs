using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class SentenceExtensions
    {
        public static int IndexOfWordInPosition(this InfoUnaFrase sentence, int wordPos)
        {
            return sentence.Palabras
                .Take(wordPos - 1)
                .Sum(w => string.IsNullOrEmpty(w.PosMark) ? w.Palabra.Length + 1 : w.Palabra.Length);
        }
    }
}