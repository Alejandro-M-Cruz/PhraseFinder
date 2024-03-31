using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.FileReaders;

namespace PhraseFinder.Domain.Tests.Services.FileReaders;

public class DleTxtPhraseDictionaryFileReaderTests : IDisposable
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
    public async Task ReadPhraseEntriesAsync_FromFileWithNoPhrases_ReturnsNoPhraseEntries(string phraseDictionaryContent)
    {
        _tempFilePath = await TestUtils.WriteToTempFileAsync(phraseDictionaryContent);
        var reader = new DleTxtPhraseDictionaryFileReader(_tempFilePath);
        List<PhraseEntry> actualPhrases = [];

        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhrases.Add(phraseEntry);
        }

        Assert.Empty(actualPhrases);
    }

    [Fact]
    public async Task ReadPhraseEntriesAsync_FromOneEntryWithTwoPhrases_ReturnsBothPhraseEntries()
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
        var reader = new DleTxtPhraseDictionaryFileReader(_tempFilePath);
        List<PhraseEntry> expectedPhraseEntries =
        [
            new()
            {
                Name = "como tamboril en boda",
                BaseWord = "tamboril",
                Categories = { "expr. coloq." },
                DefinitionToExamples =
                {
                    {
                        "1. expr. coloq. U. para expresar que algo seguramente no ha de faltar.",
                       ["Ejemplo de tamboril en boda"]
                    }
                }
            },

            new()
            {
                Name = "tamboril por gaita",
                BaseWord = "tamboril",
                Categories = { "expr. coloq." },
				DefinitionToExamples =
                {
                    {
                        "1. expr. coloq. U. para indicar que lo mismo le da a alguien una cosa que otra.",
                        []
                    }
                }
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
    public async Task ReadPhraseEntriesAsync_FromTwoEntriesWithOneAndTwoPhrases_ReturnsAllThreePhrases()
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
        var reader = new DleTxtPhraseDictionaryFileReader(_tempFilePath);
        List<PhraseEntry> expectedPhraseEntries =
        [
            new PhraseEntry
            {
                Name = "beber alguien como una topinera",
                BaseWord = "topinera",
                Categories = { "loc. verb." },
                DefinitionToExamples =
                {
                    { "1. loc. verb. Beber mucho.", new string[] {} }
                }
            },
            new PhraseEntry
            {
                Name = "echar alguien la zarpa",
                BaseWord = "zarpa[1]",
                Categories = { "loc. verb. coloq." },
				DefinitionToExamples =
                {
                    {
                        "1. loc. verb. coloq. Agarrar o asir con las manos o las uñas.",
                        []
                    },
                    {
                        "2. loc. verb. coloq. Apoderarse de algo por violencia, engaño o sorpresa.",
                        ["Le echó la zarpa al último dulce."]
                    }
                }
            },
            new PhraseEntry
            {
                Name = "hacerse alguien una zarpa",
                BaseWord = "zarpa[1]",
                Categories = { "loc. verb. coloq. desus." },
				DefinitionToExamples =
                {
                    {
                        "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho.",
                        []
                    }
                }
            }
        ];
        List<PhraseEntry> actualPhraseEntries = [];

        await foreach (var phraseEntry in reader.ReadPhraseEntriesAsync())
        {
            actualPhraseEntries.Add(phraseEntry);
        }

        Assert.Equal(expectedPhraseEntries, actualPhraseEntries);
    }

    [Fact]
    public async Task ReadPhraseEntriesAsync_WithWordsWithMultipleDefinitionsExamples_ReturnsOnlyPhraseEntriesWithTheirDefinitionsAndExamples()
    {
        string phraseDictionaryContent =
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
            "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho.\r\n" +
            "\r\n" +
            "temblar#temblar\r\n" +
            "[Etim]Del lat. vulg. tremulāre. Conjug. c. acertar.\r\n" +
            "1. intr. Agitarse con sacudidas de poca amplitud, rápidas y frecuentes.\r\n" +
            "[Sin]trepidar, retemblar, temblequear, estremecerse, tiritar, achucharse.\r\n" +
            "2. intr. Tener mucho miedo, o recelar con demasiado temor de alguien o algo. U. t. c. tr.\r\n" +
            "[Ejem]Lo tembló el universo entero.\r\n" +
            "[Sin]asustarse, amedrentarse, espantarse, temer.\r\n" +
            "[loc6]temblando\r\n" +
            "1. loc. adv. coloq. A punto de arruinarse, acabarse o concluirse.\r\n" +
            "[Ejem]Empinó la bota y la dejó temblando.\r\n" +
            "[Ejem]Segundo ejemplo.\r\n";
        _tempFilePath = await TestUtils.WriteToTempFileAsync(phraseDictionaryContent);
        var reader = new DleTxtPhraseDictionaryFileReader(_tempFilePath);
        List<PhraseEntry> expectedPhraseEntries =
        [
            new PhraseEntry
            {
                Name = "echar alguien la zarpa",
                BaseWord = "zarpa[1]",
                Categories = { "loc. verb. coloq." },
                DefinitionToExamples =
                {
                    {
                        "1. loc. verb. coloq. Agarrar o asir con las manos o las uñas.",
                        []
                    },
                    {
                        "2. loc. verb. coloq. Apoderarse de algo por violencia, engaño o sorpresa.",
                        ["Le echó la zarpa al último dulce."]
                    }
                }
            },
            new PhraseEntry
            {
                Name = "hacerse alguien una zarpa",
                BaseWord = "zarpa[1]",
                Categories = { "loc. verb. coloq. desus." },
                DefinitionToExamples =
                {
                    {
                        "1. loc. verb. coloq. desus. Mojarse o enlodarse mucho.",
                        []
                    }
                }
            },
            new PhraseEntry
            {
                Name = "temblando",
                BaseWord = "temblar",
                Categories = { "loc. adv. coloq." },
                DefinitionToExamples =
                {
                    {
                        "1. loc. adv. coloq. A punto de arruinarse, acabarse o concluirse.",
                        ["Empinó la bota y la dejó temblando.", "Segundo ejemplo."]
                    }
                }
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