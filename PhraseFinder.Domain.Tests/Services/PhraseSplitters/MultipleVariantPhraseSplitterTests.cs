using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class MultipleVariantPhraseSplitterTests
{
	private readonly MultipleVariantPhraseSplitter _splitter = new();

	[Theory]
	[InlineData("¿estamos aquí, o en Francia?")]
	[InlineData("hacer, o hacerse, cuenta, o la cuenta")]
	public void SplitPhrase_WhenPhraseHasLessThanThreeVariants_ReturnsPhrase(string phrase)
	{
		var result = _splitter.SplitPhrase(phrase);

		Assert.Single(result);
		Assert.Equal(phrase, result[0]);
	}

	[Theory]
	//[InlineData(
	//	"hacer, o hacerse, cuenta, o la cuenta",
	//	new[]
	//	{
	//		"hacer cuenta",
	//		"hacer la cuenta",
	//		"hacerse cuenta",
	//		"hacerse la cuenta"
	//	})]
	//[InlineData(
	//	"hacer, o hacerse, cuenta, o la cuenta, de algo, o alguien",
	//	new[]
	//	{
	//		"hacer cuenta de algo",
	//		"hacer cuenta de alguien",
	//		"hacer la cuenta de algo",
	//		"hacer la cuenta de alguien",
	//		"hacerse cuenta de algo",
	//		"hacerse cuenta de alguien",
	//		"hacerse la cuenta de algo",
	//		"hacerse la cuenta de alguien"
	//	})]
	[InlineData(
		"pan y circo, o pan y fútbol, o pan y toros",
		new[]
		{
			"pan y circo",
			"pan y fútbol",
			"pan y toros"
		})]
	[InlineData(
		"dale que dale, o que le das, o que le darás",
		new[]
		{
			"dale que dale",
			"dale que le das",
			"dale que le darás"
		})]
	[InlineData(
		"allá se las haya, o se las hayan, o se lo haya, o se lo hayan, o te las hayas, o te lo hayas",
		new[]
		{
			"allá se las haya",
			"allá se las hayan",
			"allá se lo haya",
			"allá se lo hayan",
			"allá te las hayas",
			"allá te lo hayas"
		})]
	[InlineData(
		"al, o con, o con el, objeto de",
		new[]
		{
			"al objeto de",
			"con objeto de",
			"con el objeto de"
		})]
	public void SplitPhrase_WhenPhraseHasThreeOrMoreVariants_ReturnsAllPossiblePhrases(
		string phrase,
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}
}