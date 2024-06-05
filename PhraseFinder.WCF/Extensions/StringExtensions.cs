using ProcesarTextos;

namespace PhraseFinder.WCF.Extensions
{
    public static class StringExtensions
    {
        public static Paragraph[] GetParagraphs(this string text)
        {
            var textProcessor = new ProcesarTextos.Text(string.Empty, text);
            return textProcessor.GetParagraphs();
        }
        
        public static bool EqualsIgnoreCase(this string str, string other)
        {
            return str.Equals(other, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}