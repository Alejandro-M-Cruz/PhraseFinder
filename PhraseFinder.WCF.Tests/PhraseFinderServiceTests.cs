using PhraseFinder.WCF.Tests.Fixtures;

namespace PhraseFinder.WCF.Tests;

public class PhraseFinderServiceTests(ServiceFixture fixture) : IClassFixture<ServiceFixture>
{
    [Fact]
    public async Task Test1()
    {
        var foundPhrases = await fixture.Service.FindPhrasesAsync("por cierto");
        var foundPhrase = foundPhrases[0];

        Assert.Single(foundPhrases);
        Assert.Equal("por cierto", foundPhrase.Phrase);
        Assert.Equal(2, foundPhrase.DefinitionToExamples.Count);
        Assert.Empty(foundPhrase.DefinitionToExamples["1. loc. adv. Ciertamente, a la verdad."]);
    }
}