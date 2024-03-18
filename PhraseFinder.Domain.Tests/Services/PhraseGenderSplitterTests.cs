using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseGenderSplitterTests
{
    private readonly PhraseGenderSplitter _phraseGenderSplitter = new();

    [Theory]
    [InlineData("a buena cuenta")]
    [InlineData("a, o por, la inversa")]
    [InlineData("lanzarse, salir, o saltar, a la palestra")]
    [InlineData("encendérsele, iluminársele, o prendérsele, la bombilla a alguien")]
    public void SplitPhrase_WhenPhraseHasNoGender_ReturnsPhrase(string phrase)
    {
        var result = _phraseGenderSplitter.SplitPhrase(phrase);

        Assert.Single(result);
        Assert.Equal(phrase, result[0]);
    }

    [Theory]
    [InlineData(
        "alguno, na que otro, tra",
        "alguno que otro", "alguna que otra")]
    [InlineData(
        "algunos, nas que otros, tras",
               "algunos que otros", "algunas que otras")]
    [InlineData(
        "muy señor, ra mío, mía",
        "muy señor mío", "muy señora mía")]
    [InlineData(
        "muy señores, ras",
        "muy señores", "muy señoras")]
    [InlineData(
        "presidente, ta", 
        "presidente", "presidenta")]
    [InlineData(
        "ni muerto, ta",
        "ni muerto", "ni muerta")]
    public void SplitPhrase_WhenPhraseHasGender_ReturnsPhrases(
        string phrase, 
        string expectedPhrase1,
        string expectedPhrase2)
    {
        var result = _phraseGenderSplitter.SplitPhrase(phrase);

        Assert.Equal(new[] { expectedPhrase1, expectedPhrase2 }, result);
    }
}