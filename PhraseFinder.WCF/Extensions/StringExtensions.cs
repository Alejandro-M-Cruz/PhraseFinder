using System.Collections.Generic;
using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF.Extensions
{
    public static class StringExtensions
    {
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
    }
}