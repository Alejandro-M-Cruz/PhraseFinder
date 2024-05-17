using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp;

internal class PhraseFinderServiceDev : IPhraseFinderService
{
	public async Task<FoundPhrase[]> FindPhrasesAsync(string text)
	{
		await Task.Delay(100);

		return text switch
		{
			"123" => [],
			"error" => throw new Exception("Error de prueba"),
			_ => new[] { '<', '>', '&' }.Any(text.Contains) ? [] :
			[
				new FoundPhrase
				{
					Phrase = "sentarse alguien a la mesa",
					StartIndex = 0,
					EndIndex = 21,
					Length = 21,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{ "1. loc. verb. Sentarse, para comer, junto a la mesa destinada al efecto.", [] },
					}
				},
				new FoundPhrase
				{
					Phrase = "darse cuenta de algo",
					StartIndex = 24,
					EndIndex = 40,
					Length = 16,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{ "1. loc. verb. Advertirlo, percatarse de ello.", [] },
						{ "2. loc. verb. coloq. Comprenderlo, entenderlo.", [] }
					}
				},
				new FoundPhrase
				{
					Phrase = "a montones",
					StartIndex = 65,
					EndIndex = 75,
					Length = 10,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{ "1. loc. adv. coloq. En abundancia, excesivamente.", [] },
					}
				},
				new FoundPhrase
				{
					Phrase = "echar un párrafo",
					StartIndex = 119,
					EndIndex = 135,
					Length = 16,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{ "1. loc. verb. coloq. Conversar amigable y familiarmente.", [] } 
					}
				},
				new FoundPhrase
				{
					Phrase = "por cuenta propia",
					StartIndex = 184,
					EndIndex = 201,
					Length = 17,
					DefinitionToExamples = new Dictionary<string, string[]>
					{
						{
							"1. loc. adj. Dicho de una persona: Que trabaja como no asalariada o que tiene su propio negocio.", 
							[
								"Trabajadores por cuenta propia. U. t. c. loc. adv.",
								"Decidió establecerse por cuenta propia."
							]
						},
						{
							"2. loc. adv. Con independencia, sin contar con nadie.", 
							["Está decidido a obrar por cuenta propia."]
						}
					}
				},
			]
		};
	}
}