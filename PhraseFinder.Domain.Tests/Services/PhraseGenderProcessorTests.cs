using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseGenderProcessorTests
{
    private readonly PhraseGenderProcessor _phraseGenderProcessor = new();

    [Fact]
    public void ProcessPhrase_WhenPhraseHasNoGender_ReturnsPhrase()
    {
        var phrase = "a buena cuenta";

        var result = _phraseGenderProcessor.ProcessPhrase(phrase);

        Assert.Single(result);
        Assert.Equal(phrase, result[0]);
    }

    [Theory]
    [InlineData(
        "alguno, na que otro, tra",
        "alguno que otro", "alguna que otra")]
    public void ProcessPhrase_WhenPhraseHasGender_ReturnsPhrases(
        string phrase, 
        string expectedPhrase1,
        string expectedPhrase2)
    {
        var result = _phraseGenderProcessor.ProcessPhrase(phrase);

        Assert.Equal(new[] { expectedPhrase1, expectedPhrase2 }, result);
    }
}