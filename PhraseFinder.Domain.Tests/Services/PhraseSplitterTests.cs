using PhraseFinder.Domain.Services;
using Xunit.Abstractions;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseSplitterTests(ITestOutputHelper output)
{
    private readonly PhraseSplitter _phraseSplitter = new();

    [Fact]
    public void SplitPhrase_WhenPhraseHasOnlyOneVariant_ReturnsSinglePhrase()
    {
        var phrase = "a buena cuenta";
        
        var result = _phraseSplitter.SplitPhrase(phrase);
        
        Assert.Single(result);
        Assert.Equal(phrase, result[0]);
    }

    [Theory]
    [InlineData(
        "echar aceite al fuego, o en el fuego", 
        new[] { "echar aceite al fuego", "echar aceite en el fuego" })]
    [InlineData(
        "abrir alguien cuenta, o una cuenta",
        new[] { "abrir alguien cuenta", "abrir alguien una cuenta" })]
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
        new[] { "cortarle a alguien la hierba bajo los pies", "segarle a alguien la hierba bajo los pies" })]
    [InlineData(
        "de cuenta, o de cuenta y riesgo, de alguien",
        new[] { "de cuenta de alguien", "de cuenta y riesgo de alguien" })]
    public void SplitPhrase_WhenPhraseHasTwoVariants_ReturnsTwoPhrases(string phrase, string[] expectedPhrases)
    {
        var result = _phraseSplitter.SplitPhrase(phrase);

        Assert.Equal(expectedPhrases, result);
    }
}