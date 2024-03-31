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
		"de cuenta, o de cuenta y riesgo, de alguien",
		new[] { "de cuenta de alguien", "de cuenta y riesgo de alguien" })]
	[InlineData(
		"estar, o ir, aviado",
		new[] { "estar aviado", "ir aviado" })]
	[InlineData(
		"meterse de por medio, o en medio",
		new[] { "meterse de por medio", "meterse en medio" })]
	[InlineData(
		"echar aceite al fuego, o en el fuego",
		new[] { "echar aceite al fuego", "echar aceite en el fuego" })]
	[InlineData(
		"ajustar cuentas, o las cuentas, a alguien",
		new[] { "ajustar cuentas a alguien", "ajustar las cuentas a alguien" })]
	[InlineData(
		"instituir heredero, o por heredero, a alguien",
		new[] { "instituir heredero a alguien", "instituir por heredero a alguien" })]
	[InlineData(
		"de montón, o en montón",
		new[] { "de montón", "en montón" })]
	[InlineData(
		"ser alguien puro nervio, o un puro nervio",
		new[] { "ser alguien puro nervio", "ser alguien un puro nervio" })]
	public void SplitPhrase_WhenPhraseHasTwoVariants_ReturnsTwoPhrases(
		string phrase,
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}
}