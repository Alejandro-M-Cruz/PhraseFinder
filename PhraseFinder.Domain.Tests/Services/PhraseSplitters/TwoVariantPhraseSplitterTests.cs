﻿using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class TwoVariantPhraseSplitterTests
{
	private readonly TwoVariantPhraseSplitter _splitter = new();

	[Theory]
	[InlineData("¿estamos aquí, o en Francia?")]
	[InlineData("¿qué hemos de hacer, o qué le hemos de hacer, o qué se le ha de hacer?")]
	public void SplitPhrase_WhenPhraseDoesNotHaveTwoVariants_ReturnsPhrase(string phrase)
	{
		var result = _splitter.SplitPhrase(phrase);

		Assert.Single(result);
		Assert.Equal(phrase, result[0]);
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
		"pesar a alguien a, o en, oro",
		new[] { "pesar a alguien a oro", "pesar a alguien en oro" })]
	[InlineData(
		"anda, o vete, al infierno",
		new[] { "anda al infierno", "vete al infierno" })]
	[InlineData(
		"pedir alguien cuenta, o cuentas",
		new[] { "pedir alguien cuenta", "pedir alguien cuentas" })]
	[InlineData(
		"cortarle, o segarle, a alguien la hierba bajo los pies",
		new[]
		{
			"cortarle a alguien la hierba bajo los pies", 
			"segarle a alguien la hierba bajo los pies"
		})]
	[InlineData(
        "echar, o soltar los perros a alguien",
		new[] { "echar los perros a alguien", "soltar los perros a alguien" })]
	[InlineData(
        "de, o en cutiano",
		new[] { "de cutiano", "en cutiano" })]
	[InlineData(
        "a la, o al papillote",
		new[] { "a la papillote", "al papillote" })]
	[InlineData(
        "con la soga a la garganta, o al cuello",
		new[] { "con la soga a la garganta", "con la soga al cuello" })]
	[InlineData(
        "irse alguien a, o de picos pardos",
		new[] { "irse alguien a picos pardos", "irse alguien de picos pardos" })]
	[InlineData(
        "echar cuenta, o cuentas con alguien o algo",
                new[] { "echar cuenta con alguien o algo", "echar cuentas con alguien o algo" })]
	[InlineData(
        "mudar cielo, o de cielo",
		new[] { "mudar cielo", "mudar de cielo" })]
	[InlineData(
        "a vuelta de ojo, o de ojos",
        new[] { "a vuelta de ojo", "a vuelta de ojos" })]
	[InlineData(
		"ser alguien puro nervio, o un puro nervio",
		new[] { "ser alguien puro nervio", "ser alguien un puro nervio" })]
	public void SplitPhrase_WhenPhraseHasTwoVariants_ReturnsTheTwoPhrases(
		string phrase,
		string[] expectedPhrases)
	{
		var actualPhrases = _splitter.SplitPhrase(phrase);

		Assert.Equal(expectedPhrases, actualPhrases);
	}
}