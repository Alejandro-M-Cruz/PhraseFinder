using System.Diagnostics;
using PhraseFinderServiceReference;

var client = new PhraseFinderServiceClient();

var text = 
	"alguno, na que otro, tra texto de ejemplo.\n esto es un texto de ejemplo";

var stopwatch = Stopwatch.StartNew();
var foundPhrases = await client.FindPhrasesAsync(text);
stopwatch.Stop();

Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");

foreach (var phrase in foundPhrases)
{
	Console.WriteLine($"Phrase: {phrase.Phrase}");
	Console.WriteLine($"Start index: {phrase.StartIndex}");
	Console.WriteLine($"End index: {phrase.EndIndex}");
	Console.WriteLine($"Length: {phrase.Length}");

	foreach (var definitionToExamples in phrase.DefinitionToExamples)
	{
		Console.WriteLine($"Definition: {definitionToExamples.Key}");
		foreach (var example in definitionToExamples.Value)
		{
			Console.WriteLine($"Example: {example}");
		}
	}
}

