using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp;

internal class PhraseFinderServiceDev : IPhraseFinderService
{
	public async Task<FoundPhrase[]> FindPhrasesAsync(string text)
	{
		await Task.Delay(3000);

		return text switch
		{
			"123" => [],
			"error" => throw new Exception("Error de prueba"),
			_ => new[] { '<', '>', '&' }.Any(text.Contains) ? [] :
			[
				new FoundPhrase
				{
					Phrase = "sentarse alguien a la mesa",
                    BaseWord = "mesa",
					StartIndex = 0,
					EndIndex = 21,
					Length = 21,
					Definitions =
					[
					    new PhraseDefinition
                        {
							Definition = "1. loc. verb. Sentarse, para comer, junto a la mesa destinada al efecto.",
							Examples = []
                        }	
                    ]
				},
                new FoundPhrase
                {
                    Phrase = "darse cuenta de algo",
                    BaseWord = "cuenta",
                    StartIndex = 24,
                    EndIndex = 40,
                    Length = 16,
                    Definitions =
                    [
                        new PhraseDefinition
                        {
                            Definition = "1. loc. verb. Advertirlo, percatarse de ello.",
                            Examples = []
                        },
                        new PhraseDefinition
                        {
                            Definition = "2. loc. verb. coloq. Comprenderlo, entenderlo.",
                            Examples = []
                        }
                    ]
                },
                new FoundPhrase
                {
                    Phrase = "a montones",
                    BaseWord = "montón",
                    StartIndex = 65,
                    EndIndex = 75,
                    Length = 10,
                    Definitions =
                    [
                        new PhraseDefinition
                        {
                            Definition = "1. loc. adv. coloq. En abundancia, excesivamente.",
                            Examples = []
                        }
                    ]
                },
                new FoundPhrase
                {
                    Phrase = "echar un párrafo",
                    BaseWord = "párrafo",
                    StartIndex = 119,
                    EndIndex = 135,
                    Length = 16,
                    Definitions =
                    [
                        new PhraseDefinition
                        {
                            Definition = "1. loc. verb. coloq. Conversar amigable y familiarmente.",
                            Examples = []
                        }
                    ]
                },
                new FoundPhrase
                {
                    Phrase = "por cuenta propia",
                    BaseWord = "cuenta",
                    StartIndex = 184,
                    EndIndex = 201,
                    Length = 17,
                    Definitions =
                    [
                        new PhraseDefinition
                        {
                            Definition = "1. loc. adj. Dicho de una persona: Que trabaja como no asalariada o que tiene su propio negocio.",
                            Examples =
                            [
                                "Trabajadores por cuenta propia. U. t. c. loc. adv.",
                                "Decidió establecerse por cuenta propia."
                            ]
                        },
                        new PhraseDefinition
                        {
                            Definition = "2. loc. adv. Con independencia, sin contar con nadie.",
                            Examples = ["Está decidido a obrar por cuenta propia."]
                        }
                    ]
                }
			]
		};
	}
}