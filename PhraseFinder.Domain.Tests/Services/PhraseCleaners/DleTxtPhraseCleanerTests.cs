using PhraseFinder.Domain.Services.PhraseCleaners;

namespace PhraseFinder.Domain.Tests.Services.PhraseCleaners;

public class DleTxtPhraseCleanerTests
{
    private readonly DleTxtPhraseCleaner _phraseCleaner = new();

    [Fact]
    public void CleanPhrase_WhenPhraseDoesNotContainUnwantedCharacters_ReturnsPhrase()
    {
        var phrase = "dar bandazos";

        var result = _phraseCleaner.CleanPhrase(phrase);

        Assert.Equal(phrase, result);
    }

    [Theory]
    [InlineData(
        "sobre manera V. sobremanera.",
        "sobre manera")]
    [InlineData(
        "a Roma por todo",
        "a Roma por todo")]
    [InlineData(
        "saber más que Lepe, o que Lepe, Lepijo y su hijo Por alus. a Pedro de Lepe, 1641-1700, obispo de Calahorra y La Calzada, autor de un conocido catecismo.",
        "saber más que Lepe, o que Lepe, Lepijo y su hijo")]
    [InlineData(
        "a buena cuenta",
        "a buena cuenta")]
    [InlineData(
        "dar algo, especialmente la ropa, de sí",
        "dar algo de sí")]
    [InlineData(
        "hacer novillos alguien, especialmente un escolar",
        "hacer novillos alguien")]
    [InlineData(
        "a la carbonara Del it. alla car" +
        "bonara.", "a la carbonara")]
    [InlineData(
        "tarde piache Del gall. tarde piache, 'tarde piaste', frase que la tradición atribuye a un soldado que, al tragarse un huevo empollado, oyó piar al polluelo.",
        "tarde piache")]
    [InlineData(
        "viva la Pepa Expr. con que se celebraba la Constitución española de 1812, llamada popularmente así por haberse promulgado el día de san José y ser Pepa el hipocorístico de Josefa.",
        "viva la Pepa")]
    [InlineData(
        "valer algo un perú Escr. t. con may. inicial,",
        "valer algo un perú")]
    [InlineData(
        "a portagayola De la loc. port. a porta gaiola; literalmente 'a puerta[del] toril'.",
        "a portagayola")]
    [InlineData(
        "dares y tomares De dar y tomar.",
        "dares y tomares")]
    [InlineData(
        "al alimón Falsa separación de alalimón.",
        "al alimón")]
    [InlineData(
        "cochite hervite Quizá alterac. del b. lat. coquite, fervite 'coced, hervid', a imit. de arate, cavate 'arad, cavad'.",
        "cochite hervite")]
    public void CleanPhrase_WhenPhraseContainsUnwantedCharacters_ReturnsCleanPhrase(
        string phrase,
        string expectedResult)
    {
        var result = _phraseCleaner.CleanPhrase(phrase);

        Assert.Equal(expectedResult, result);
    }
}