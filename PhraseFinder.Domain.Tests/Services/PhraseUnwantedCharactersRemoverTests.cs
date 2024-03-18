using PhraseFinder.Domain.Services;

namespace PhraseFinder.Domain.Tests.Services;

public class PhraseUnwantedCharactersRemoverTests
{
    [Fact]
    public void RemoveVideSection_WhenPhraseDoesNotContainVideSection_ReturnsPhrase()
    {
        var phrase = "dar bandazos";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveVideSection(phrase);

        Assert.Equal(phrase, result);
    }

    [Fact]
    public void RemoveVideSection_WhenPhraseContainsVideSection_ReturnsPhraseWithoutVideSection()
    {
        var phrase = "sobre manera V. sobremanera.";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveVideSection(phrase);

        Assert.Equal("sobre manera", result);
    }

    [Fact]
    public void RemoveByAllusionSection_WhenPhraseDoesNotContainByAllusionSection_ReturnsPhrase()
    {
        var phrase = "a Roma por todo";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveByAllusionSection(phrase);

        Assert.Equal(phrase, result);
    }

    [Fact]
    public void RemoveByAllusionSection_WhenPhraseContainsByAllusionSection_ReturnsPhraseWithoutByAllusionSection()
    {
        var phrase =
            "saber más que Lepe, o que Lepe, Lepijo y su hijo Por alus. a Pedro de Lepe, 1641-1700, " +
            "obispo de Calahorra y La Calzada, autor de un conocido catecismo.";
        var phraseUnwantedCharactersRemover = new PhraseUnwantedCharactersRemover();

        var result = phraseUnwantedCharactersRemover.RemoveByAllusionSection(phrase);

        Assert.Equal("saber más que Lepe, o que Lepe, Lepijo y su hijo", result);
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