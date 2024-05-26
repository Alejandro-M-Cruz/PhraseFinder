using System;
using System.Collections.Generic;
using System.Linq;
using PhraseFinder.WCF.ServicioLematizacion;
using ProcesarTextos;

namespace PhraseFinder.WCF.Extensions
{
    public static class ParagraphExtensions
    {
        public static IEnumerable<InfoUnaFrase> SelectAllSentences(this IEnumerable<Paragraph> paragraphs)
        {
            return paragraphs.SelectMany(p => p.GetSentences()).Select(s => new InfoUnaFrase
            {
                Frase = s.getText(),
                Palabras = s.GetWords().Select(w => new InfoUnaPalabra()
                {
                    Palabra = w.ToString()
                }).ToList()
            });
        }

        public static string ReconstructText(
            this IEnumerable<Paragraph> paragraphs, 
            string paragraphSeparator, 
            string sentenceSeparator)
        {
            return string.Join(
                paragraphSeparator,
                paragraphs.Select(p => 
                    string.Join(sentenceSeparator, p.GetSentences().Select(s => s.getText()))));
        }
    }
}