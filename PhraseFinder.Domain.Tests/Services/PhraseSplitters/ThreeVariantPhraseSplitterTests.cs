using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class ThreeVariantPhraseSplitterTests
{
    private readonly ThreeVariantPhraseSplitter _splitter = new();

    [Fact]
    public void SplitPhrase_WhenPhraseDoesNotHaveThreeVariants_ReturnsPhrase()
    {
        var phrase = "llevar, o traer algo traza";

        var result = _splitter.SplitPhrase(phrase);

        Assert.Single(result);
        Assert.Equal(phrase, result[0]);
    }

    [Theory]
    [InlineData(
        "¿qué cuentas?, ¿qué cuenta usted?, o ¿qué cuentan ustedes?",
        new[] { "¿qué cuentas?", "¿qué cuenta usted?", "¿qué cuentan ustedes?" })]
    [InlineData(
        "decir algo con la boca chica, chiquita, o pequeña",
        new[] { "decir algo con la boca chica", "decir algo con la boca chiquita", "decir algo con la boca pequeña" })]
    [InlineData(
        "menear, sacudir, o zurrar a alguien el bálago",
        new[] { "menear a alguien el bálago", "sacudir a alguien el bálago", "zurrar a alguien el bálago" })]
    [InlineData(
        "a la zaga, a zaga, o en zaga",
        new[] { "a la zaga", "a zaga", "en zaga" })]
    //[InlineData(
    //    "tener el colmillo retorcido, tener colmillos, o colmillos retorcidos",
    //    new[] { "tener el colmillo retorcido", "tener colmillos", "tener colmillos retorcidos" })]
    public void SplitPhrase_WhenPhraseHasThreeVariants_ReturnsTheThreePhrases(
        string phrase, string[] expectedPhrases)
    {
        var actualPhrases = _splitter.SplitPhrase(phrase);

        Assert.Equal(expectedPhrases, actualPhrases);
    }
}