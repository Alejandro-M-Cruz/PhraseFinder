using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp;

internal class PhraseFinderServiceDev : IPhraseFinderService
{
	public async Task<FoundPhrase[]> FindPhrasesAsync(string text)
	{
		await Task.Delay(2000);
		return
		[
			new FoundPhrase
			{
				Phrase = "alguno, na que otro, tra",
				StartIndex = 0,
				EndIndex = 10,
				Length = 10
			},
			new FoundPhrase
			{
				Phrase = "alguno, na que otro, tra",
				StartIndex = 33,
				EndIndex = 39,
				Length = 6,
			},
			new FoundPhrase
			{
				Phrase = "texto de ejemplo",
				StartIndex = 44,
				EndIndex = 55,
				Length = 11
			}
		];
	}
}