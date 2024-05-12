using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class EtcPhraseSplitterTests
{
	private readonly EtcPhraseSplitter _splitter = new();

	[Theory]
	[InlineData("¿qué hemos de hacer, o qué le hemos de hacer, o qué se le ha de hacer?")]
	[InlineData("ejemplo, etcétera, etcétera")]
	public void SplitPhrase_WhenPhraseDoesNotContainEtc_ReturnsPhrase(string phrase)
	{
		var result = _splitter.SplitPhrase(phrase);

		Assert.Single(result);
		Assert.Equal(phrase, result[0]);
	}

	[Theory]
	[InlineData(
		"por mi, etc., cuenta",
		new[] { "por mi cuenta" })]
	[InlineData(
		"mi, tu, su, etc., cuenta",
				new[] { "mi cuenta", "tu cuenta", "su cuenta" })]
	[InlineData(
		"por mi, tu, su, etc., cuenta",
		new[] { "por mi cuenta", "por tu cuenta", "por su cuenta" })]
	[InlineData(
		"tener medido a palmos un terreno, un lugar, etc.",
		new[] { "tener medido a palmos un terreno", "tener medido a palmos un lugar" } )]
	[InlineData(
		"por mi, tu, su, etc., bella, o linda, cara",
		new[]
		{
			"por mi bella, o linda, cara", 
			"por tu bella, o linda, cara", 
			"por su bella, o linda, cara"
		})]
	[InlineData(
		"mira, o mira tú, o mire, etc., por cuánto",
		new[] { "mira por cuánto", "mira tú por cuánto", "mire por cuánto" })]
	[InlineData(
		"ahí, o allí, o aquí, fue, o será, etc., Troya",
		new[] {"ahí, o allí, o aquí, fue Troya", "ahí, o allí, o aquí, será Troya" })]
	[InlineData(
		"más eres tú, más es él, etc.",
		new[] { "más eres tú", "más es él" })]
	public void SplitPhrase_WhenPhraseContainsEtc_ReturnsAllVariants(
		string phrase, 
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}
}