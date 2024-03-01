using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;
using Xunit.Abstractions;

namespace PhraseFinder.Domain.Tests.Services;

public class DleTxtPhraseDictionaryReaderTests(ITestOutputHelper output) : IDisposable
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
        List<Phrase> actualPhrases = [];
        
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
        List<Phrase> expectedPhraseEntries =
        [
            new Phrase
            {
                Name = "como tamboril en boda", 
                BaseWord = "tamboril",
                Definitions =
                {
                    new PhraseDefinition
                    {
                        Definition = "1. expr. coloq. U. para expresar que algo seguramente no ha de faltar."
                    }
                },
                Examples = { new PhraseExample { Example = "Ejemplo de tamboril en boda" } }
            },
            new Phrase
            {
                Name = "tamboril por gaita", 
                BaseWord = "tamboril",
                Definitions = 
                {
                    new PhraseDefinition
                    {
                        Definition = "1. expr. coloq. U. para indicar que lo mismo le da a alguien una cosa que otra."
                    }
                }
            }
        ];
        List<Phrase> actualPhrases = [];
        
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
        List<Phrase> expectedPhrases =
        [
            new Phrase
            {
                Name = "beber alguien como una topinera",
                BaseWord = "topinera",
                Definitions = { new PhraseDefinition { Definition = "1. loc. verb. Beber mucho." } }
            },
            new Phrase
            {
                Name = "echar alguien la zarpa",
                BaseWord = "zarpa[1]",
                Definitions =
                {
                    new PhraseDefinition { Definition = "1. loc. verb. coloq. Agarrar o asir con las manos o las uñas." },
                    new PhraseDefinition { Definition = "2. loc. verb. coloq. Apoderarse de algo por violencia, engaño o sorpresa." }
                },
                Examples = { new PhraseExample { Example = "Le echó la zarpa al último dulce." } }
            },
            new Phrase
            {
                Name = "hacerse alguien una zarpa", 
                BaseWord = "zarpa[1]",
                Definitions = { new PhraseDefinition { Definition = "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho." } }
            }
        ];
        List<Phrase> actualPhrases = [];
        
        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhrases.Add(phraseEntry);
            output.WriteLine(phraseEntry.ToString());
        }

        Assert.Equal(expectedPhrases, actualPhrases);
    }

    public void Dispose()
    {
        if (_tempFilePath != null)
        {
            File.Delete(_tempFilePath);
        }
    }
}