using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;
using Xunit.Abstractions;

namespace PhraseFinder.Domain.Tests.Services;

public class DleTxtPhraseDictionaryReaderTests : IDisposable
{
    private string? _tempFilePath;
    
    [Theory]
    [InlineData("")]
    [InlineData(
        "tejaroz#tejaroz\r\n" +
        "[Etim]Der. de teja[1].\r\n" +
        "1. m. Alero del tejado.\r\n" +
        "[Sin]alero, saliente.\r\n" +
        "2. m. Tejadillo construido sobre una puerta o ventana.")]
    public async Task ReadPhrasesAsync_FromFileWithNoPhrases_ReturnsNoPhraseEntries(string phraseDictionaryContent)
    {
        _tempFilePath = await TestUtils.WriteToTempFileAsync(phraseDictionaryContent);
        var reader = new DleTxtPhraseDictionaryReader(_tempFilePath);
        List<PhraseEntry> actualPhrases = [];
        
        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhrases.Add(phraseEntry);
        }

        Assert.Empty(actualPhrases);
    }
    
    [Fact]
    public async Task ReadPhrasesAsync_FromOneEntryWithTwoPhrases_ReturnsBothPhraseEntries()
    {
        string phraseDictionaryContent =
            "tamboril#tamboril\r\n" +
            "[Etim]De tamborín.\r\n" +
            "1. m. Tambor pequeño que, colgado del brazo, se toca con un solo palillo o baqueta, y, acompañando " +
            "generalmente al pito, se usa en algunas danzas populares.\r\n" +
            "[Sin]tamborín, tamborino, atabal, timbal, tambor.\r\n" +
            "[loc6]como tamboril en boda\r\n" +
            "1. expr. coloq. U. para expresar que algo seguramente no ha de faltar.\r\n" +
            "[Ejem]Ejemplo de tamboril en boda\r\n" +
            "[loc6]tamboril por gaita\r\n" +
            "1. expr. coloq. U. para indicar que lo mismo le da a alguien una cosa que otra.\r\n";
        _tempFilePath = await TestUtils.WriteToTempFileAsync(phraseDictionaryContent);
        var reader = new DleTxtPhraseDictionaryReader(_tempFilePath);
        List<PhraseEntry> expectedPhraseEntries =
        [
            new()
            {
                Name = "como tamboril en boda",
                BaseWord = "tamboril",
                Definitions = { "1. expr. coloq. U. para expresar que algo seguramente no ha de faltar." },
                Examples = { "Ejemplo de tamboril en boda" }
            },

            new()
            {
                Name = "tamboril por gaita",
                BaseWord = "tamboril",
                Definitions = { "1. expr. coloq. U. para indicar que lo mismo le da a alguien una cosa que otra." }
            }
        ];
        List<PhraseEntry> actualPhrases = [];

        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhrases.Add(phraseEntry);
        }

        Assert.Equal(expectedPhraseEntries, actualPhrases);
    }

    [Fact]
    public async Task ReadPhrasesAsync_FromTwoEntriesWithOneAndTwoPhrases_ReturnsAllThreePhrases()
    {
        string phraseDictionaryContent =
            "topinera#topinera\r\n" +
            "1. f. Madriguera del topo.\r\n" +
            "[loc6]beber alguien como una topinera\r\n" +
            "1. loc. verb. Beber mucho.\r\n" +
            "\r\n" +
            "zarpa#zarpa[1]\r\n" +
            "[Etim]Del ant. farpa 'pingajo, jirón', infl. por el sinónimo zarria.\r\n" +
            "1. f. Mano de ciertos animales cuyos dedos no se mueven con independencia unos de otros, " +
            "como en el león y el tigre.\r\n" +
            "[Sin]garra, mano, pezuña.\r\n" +
            "2. f. Lodo o barro que se queda en la parte baja de la ropa.\r\n" +
            "[loc6]echar alguien la zarpa\r\n" +
            "1. loc. verb. coloq. Agarrar o asir con las manos o las uñas.\r\n" +
            "2. loc. verb. coloq. Apoderarse de algo por violencia, engaño o sorpresa.\r\n" +
            "[Ejem]Le echó la zarpa al último dulce.\r\n" +
            "[loc6]hacerse alguien una zarpa\r\n" +
            "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho.\r\n";
        _tempFilePath = await TestUtils.WriteToTempFileAsync(phraseDictionaryContent);
        var reader = new DleTxtPhraseDictionaryReader(_tempFilePath);
        List<PhraseEntry> expectedPhraseEntries =
        [
            new PhraseEntry
            {
                Name = "beber alguien como una topinera",
                BaseWord = "topinera",
                Definitions = { "1. loc. verb. Beber mucho." }
            },
            new PhraseEntry
            {
                Name = "echar alguien la zarpa",
                BaseWord = "zarpa[1]",
                Definitions =
                {
                    "1. loc. verb. coloq. Agarrar o asir con las manos o las uñas.",
                    "2. loc. verb. coloq. Apoderarse de algo por violencia, engaño o sorpresa."
                },
                Examples = { "Le echó la zarpa al último dulce." }
            },
            new PhraseEntry
            {
                Name = "hacerse alguien una zarpa",
                BaseWord = "zarpa[1]",
                Definitions = { "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho." }
            }
        ];
        List<PhraseEntry> actualPhraseEntries = [];

        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhraseEntries.Add(phraseEntry);
        }

        Assert.Equal(expectedPhraseEntries, actualPhraseEntries);
    }

    public void Dispose()
    {
        if (_tempFilePath != null)
        {
            File.Delete(_tempFilePath);
        }
    }
}