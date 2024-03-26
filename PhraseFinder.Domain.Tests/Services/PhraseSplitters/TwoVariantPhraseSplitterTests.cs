using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class TwoVariantPhraseSplitterTests
{
	private readonly TwoVariantPhraseSplitter _splitter = new();

	[Theory]
	[InlineData(
		"¿estamos aquí, o en Francia ?",
		new[] { "¿estamos aquí, o en Francia ?" })]
	[InlineData(
		"¿qué hemos de hacer, o qué le hemos de hacer, o qué se le ha de hacer?",
		new[] { "¿qué hemos de hacer, o qué le hemos de hacer, o qué se le ha de hacer?" })]

	public void SplitPhrase_WhenPhraseDoesNotHaveTwoVariants_ReturnsPhrase(
		string phrase, 
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}

	[Theory]
	[InlineData(
		"¿no es buñuelo?, o ¿no son buñuelos?",
		new[] { "¿no es buñuelo?", "¿no son buñuelos?" })]
	[InlineData(
		"abrir alguien cuenta, o una cuenta",
		new[] { "abrir alguien cuenta", "abrir alguien una cuenta" })]
	[InlineData("tirar alguien a ventana conocida, o señalada",
		new[] { "tirar alguien a ventana conocida", "tirar alguien a ventana señalada" })]
	[InlineData(
		"por en medio, o por medio",
		new[] { "por en medio", "por medio" })]
	[InlineData(
		"meterse de por medio, o en medio",
		new[] { "meterse de por medio", "meterse en medio" })]
	[InlineData(
		"de cuenta, o de cuenta y riesgo, de alguien",
		new[] { "de cuenta de alguien", "de cuenta y riesgo de alguien" })]
	public void SplitPhrase_WhenPhraseHasTwoVariants_ReturnsTwoPhrases(
		string phrase,
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}
}