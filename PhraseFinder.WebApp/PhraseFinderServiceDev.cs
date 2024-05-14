using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp;

internal class PhraseFinderServiceDev : IPhraseFinderService
{
	public async Task<FoundPhrase[]> FindPhrasesAsync(string text)
	{
		await Task.Delay(2000);

		return text switch
		{
			"123" => [],
			"error" => throw new Exception("Error de prueba"),
			_ =>
			[
				new FoundPhrase
				{
					Phrase = "alguno, na que otro, tra",
					StartIndex = 0,
					EndIndex = 10,
					Length = 10,
					DefinitionToExamples =
						new Dictionary<string, string[]>
						{
							{ "Definición de ejemplo", ["Ejemplo 1", "Ejemplo 2"] },
							{ "Definición de ejemplo 2", ["Ejemplo 3", "Ejemplo 4"] }
						}
				},
				new FoundPhrase
				{
					Phrase = "alguno, na que otro, tra",
					StartIndex = 33,
					EndIndex = 39,
					Length = 6,
					DefinitionToExamples =
						new Dictionary<string, string[]> { { "Definición de ejemplo", ["Ejemplo 1", "Ejemplo 2"] } }
				},
				new FoundPhrase
				{
					Phrase = "alguno, na que otro, tra",
					StartIndex = 33,
					EndIndex = 39,
					Length = 6,
					DefinitionToExamples =
						new Dictionary<string, string[]> { { "Definición de ejemplo", ["Ejemplo 1", "Ejemplo 2"] } }
				},
				new FoundPhrase
				{
					Phrase = "texto de ejemplo",
					StartIndex = 44,
					EndIndex = 55,
					Length = 11,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{ "Definición de ejemplo", ["Ejemplo 1", "Ejemplo 2"] }
					}
				}
			]
		};
	}
}