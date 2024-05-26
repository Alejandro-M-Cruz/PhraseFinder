using ServicioLematizacion;

namespace PhraseFinder.WCF.PerformanceTests;

public static class Utils
{
    public static async Task PrintInfo(string text)
    {
        var textProcessor = new ProcesarTextos.Text(string.Empty, text);
        var paragraphs = textProcessor.GetParagraphs().ToArray();
        var sentences = paragraphs.SelectMany(p => p.GetSentences()).ToArray();

        var frases = sentences.Select(s => new InfoUnaFrase()
        {
            Frase = s.getText(),
            Palabras = s.GetWords().Select(w => new InfoUnaPalabra()
            {
                Palabra = w.ToString()
            }).ToArray()
        });

        var servicioLematizacion = new ServicioLematizacionClient();

        frases = await servicioLematizacion.NuevoReconocerFrasesAsync(frases.ToArray(), "es", multiPref: false);

        foreach (var frase in frases)
        {
            foreach (var palabra in frase.Palabras)
            {
                Console.WriteLine($"Frase: {frase.Frase}");
                Console.WriteLine($"Palabra: {palabra.Palabra}");
                Console.WriteLine($"Forma canónica: {palabra.FormaCanonica}");
                Console.WriteLine($"Categoría: {palabra.IdCategoria}");
                Console.WriteLine($"Posición: {palabra.Posicion}");
                Console.WriteLine($"PosMark: {palabra.PosMark}");
                Console.WriteLine($"Prefijos: {palabra.Prefijos}");
                Console.WriteLine($"Pronombres: {palabra.Pronombres}");
                Console.WriteLine($"Frecuencia: {palabra.Frecuencia}");
                Console.WriteLine();
            }
        }

        Console.WriteLine("Paragraphs from text processor:");

        foreach (var paragraph in paragraphs)
        {
            Console.WriteLine($"Paragraph: {paragraph.getText()}");
        }

        Console.WriteLine("Sentences from text processor:");

        foreach (var sentence in sentences)
        {
            Console.WriteLine($"Sentence: {sentence.getText()}");
        }

        Console.WriteLine();

        foreach (var frase in frases)
        {
            Console.WriteLine($"Frase: {frase.Frase}");
        }
    }
}