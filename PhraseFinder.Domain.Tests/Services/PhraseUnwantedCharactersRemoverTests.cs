using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseUnwantedCharactersRemoverTests
{
    [Fact]
    public void RemoveVide_WhenPhraseDoesNotContainVide_ReturnsPhrase()
    {
        var phrase = "a buena cuenta";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveVide(phrase);

        Assert.Equal(phrase, result);
    }

    [Fact]
    public void RemoveVide_WhenPhraseContainsVide_ReturnsPhraseWithoutVide()
    {
        var phrase = "sobre manera V. sobremanera.";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveVide(phrase);

        Assert.Equal("sobre manera", result);
    }

    [Fact]
    public void RemoveSpecification_WhenPhraseDoesNotContainOne_ReturnsPhrase()
    {
        var phrase = "a buena cuenta";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();
        
        var result = phraseUnwantedCharactersRemover.RemoveSpecification(phrase);

        Assert.Equal(phrase, result);
    }

    [Theory]
    [InlineData(
        "dar algo, especialmente la ropa, de sí", 
        "dar algo de sí")]
    [InlineData(
        "hacer novillos alguien, especialmente un escolar", 
        "hacer novillos alguien")]
    public void RemoveSpecification_WhenPhraseContainsOne_ReturnsPhraseWithoutSpecification(
        string phrase, 
        string expectedResult)
    {
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveSpecification(phrase);

        Assert.Equal(expectedResult, result);
    }
}