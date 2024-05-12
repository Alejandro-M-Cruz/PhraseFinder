using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Tests.Services.PhraseSplitters;

public class TbPhraseSplitterTests
{
    private readonly TbPhraseSplitter _splitter = new();

    [Fact]
    public void SplitPhrase_WhenPhraseDoesNotContainTb_ReturnsPhrase()
    {
        var phrase = "de cuenta, o de cuenta y riesgo, de alguien";

        var result = _splitter.SplitPhrase(phrase);

        Assert.Single(result);
		Assert.Equal(phrase, result[0]);
    }

    [Theory]
    [InlineData(
        "hacer fuerarropa Tb. hacer fuera ropa.",
        new[] { "hacer fuerarropa", "hacer fuera ropa" })]
    [InlineData(
        "a rajatabla Tb. a raja tabla, p. us.",
        new[] { "a rajatabla", "a raja tabla" })]
    [InlineData(
        "a espetaperro o espetaperros Tb. a espetaperros; a espeta perros, desus.",
    new[] { "a espetaperro o espetaperros", "a espetaperros", "a espeta perros" })]

    public void SplitPhrase_WhenPhraseContainsTb_ReturnsAllPossiblePhrases(
        string phrase,
        string[] expected)
    {
        var result = _splitter.SplitPhrase(phrase);

        Assert.Equal(expected, result);
    }
}