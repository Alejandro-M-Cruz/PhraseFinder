using System.Diagnostics;
using PhraseFinder.WCF.PerformanceTests;
using PhraseFinderServiceReference;
using ServicioLematizacion;

//var client = new PhraseFinderServiceClient();

//var text = 
//	"alguno, na que otro, tra texto de ejemplo.\n esto es un texto de ejemplo";

//var stopwatch = Stopwatch.StartNew();
//var foundPhrases = await client.FindPhrasesAsync(text);
//stopwatch.Stop();

//Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

//foreach (var phrase in foundPhrases)
//{
//	Console.WriteLine($"Phrase: {phrase.Phrase}");
//	Console.WriteLine($"Start index: {phrase.StartIndex}");
//	Console.WriteLine($"End index: {phrase.EndIndex}");
//	Console.WriteLine($"Length: {phrase.Length}");

//	foreach (var definitionToExamples in phrase.DefinitionToExamples)
//	{
//		Console.WriteLine($"Definition: {definitionToExamples.Key}");
//		foreach (var example in definitionToExamples.Value)
//		{
//			Console.WriteLine($"Example: {example}");
//		}
//	}
//}

// text of example

const string sampleText = 
    "Se sentaron a la mesa y se dieron cuenta enseguida. Había comida a montones. " + 
    "Al poco tiempo, comenzaron  a almorzar y a echar un párrafo. " + 
    "¿Cabría la posibilidad de ...?\n" +
    "El primero en irse fue Juan, que siempre actúa por cuenta propia.";

var textProcessor = new ProcesarTextos.Text(string.Empty, sampleText);
var paragraphs = textProcessor.GetParagraphs().ToArray();
var sentences = paragraphs.SelectMany(p => p.GetSentences()).ToArray();


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

//foreach (var frase in frases)
//{
//    Console.WriteLine($"Frase: {frase.Frase}");
//    foreach (var palabra in frase.Palabras)
//    {
//        Console.WriteLine($"Palabra: {palabra.Palabra}");
//        Console.WriteLine($"Forma canónica: {palabra.FormaCanonica}");
//        Console.WriteLine($"Categoría: {palabra.IdCategoria}");
//        Console.WriteLine($"Posición: {palabra.Posicion}");
//        Console.WriteLine($"PosMark: {palabra.PosMark}");
//        Console.WriteLine();
//    }
//}

var phrases = new Phrase[]
{
    new()
    {
        PhraseId = 1,
        Value = "por cierto",
        Pattern = "por cierto",
        BaseWord = "cierto"
    },
    new()
    {
        PhraseId = 2,
        Value = "a montones",
        Pattern = "a montones",
        BaseWord = "montón"
    },
    new()
    {
        PhraseId = 3,
        Value = "dar cuenta",
        Pattern = "dar cuenta",
        BaseWord = "cuenta"
    },
};

var foundPhrases = new Dictionary<int, string>();

var IsVerb = (InfoUnaPalabra palabra) => 
    palabra.IdCategoria is > 3000 and < 3100;

var WordsCoincide = (InfoUnaPalabra palabra, string word) => 
    palabra.Palabra == word || (IsVerb(palabra) && palabra.FormaCanonica == word);

foreach (var phrase in phrases)
{
    var words = phrase.Value.Split(' ').Select(w => w.TrimEnd(',')).ToArray();
    var firstWord = words.First();
    
    foreach (var frase in frases)
    {
        foreach (var palabra in frase.Palabras)
        {
            if (WordsCoincide(palabra, firstWord))
            {
                var i = palabra.Posicion;
                if (!words.Skip(1).All(w =>
                    {
                        var p = frase.Palabras[i];
                        if (p.Posicion == 0)
                        {
                            return true;
                        }
                        i++;
                        return WordsCoincide(p, w);
                    }))
                {
                    continue;
                }
                foundPhrases[palabra.Posicion] = palabra.Palabra;
            }
        }
    }
}

Console.WriteLine("Found phrases:");

foreach (var foundPhrase in foundPhrases)
{
    Console.WriteLine($"{foundPhrase.Key}: {foundPhrase.Value}");
}
