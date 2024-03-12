using System.Text.RegularExpressions;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services;

// <summary>
// Class <c>DleTxtReader</c> is a reader for the Diccionario de la Lengua Española (DLE) dictionary.
// It should be a plain text file where each entry is separated by a blank line and has the following format:
// <example>
// tamboril#tamboril
// [Etim]De tamborín.
// 1. m. Tambor pequeño que, colgado del brazo, se toca con un solo palillo o baqueta, y, acompañando generalmente al
// pito, se usa en algunas danzas populares.
// [Sin]tamborín, tamborino, atabal, timbal, tambor.
// [loc6]como tamboril en boda
// 1. expr. coloq. U. para expresar que algo seguramente no ha de faltar.
// [loc6]tamboril por gaita
// 1. expr. coloq. U. para indicar que lo mismo le da a alguien una cosa que otra.
// </example>
// </summary>
public class DleTxtPhraseDictionaryFileFileReader(string filePath) : IPhraseDictionaryFileReader
{
    private const string PhrasePrefix = "[loc6]";
    private const string PhraseExamplePrefix = "[Ejem]";
    private static readonly Regex PhraseDefinitionRegEx = new(
        @"^\d+\.\s(loc\.|locs\.|expr\.)", RegexOptions.Compiled);

    public async IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync()
    {
        using var reader = new StreamReader(filePath);
        string? currentLine = null;
        string? currentWord = null;
        PhraseEntry? currentPhraseEntry = null;
        string? currentPhraseDefinition = null;
        while ((currentLine = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(currentLine) && currentPhraseEntry != null)
            {
                yield return currentPhraseEntry;
                currentPhraseEntry = null;
                currentPhraseDefinition = null;
            }
            else if (currentLine.Contains('#'))
            {
                currentWord = currentLine.Split('#')[1];
                currentPhraseDefinition = null;
            }
            else if (currentLine.StartsWith(PhrasePrefix))
            {
                if (currentPhraseEntry != null)
                {
                    yield return currentPhraseEntry;
                }
                currentPhraseEntry = new PhraseEntry
                {
                    Name = currentLine[PhrasePrefix.Length..],
                    BaseWord = currentWord ?? ""
                };
                currentPhraseDefinition = null;
            } 
            else if (PhraseDefinitionRegEx.IsMatch(currentLine))
            {
                currentPhraseDefinition = currentLine;
                currentPhraseEntry?.DefinitionToExamples.Add(currentPhraseDefinition, []);
            }
            else if (currentLine.StartsWith(PhraseExamplePrefix) && currentPhraseDefinition != null)
            {
                currentPhraseEntry?
                    .DefinitionToExamples[currentPhraseDefinition]
                    .Add(currentLine[PhraseExamplePrefix.Length..]);
            }
        }
        if (currentPhraseEntry != null)
        {
            yield return currentPhraseEntry;
        }
    }

    public IEnumerable<PhraseEntry> ReadPhraseEntries()
    {
        using var reader = new StreamReader(filePath);
        string? currentLine = null;
        string? currentWord = null;
        PhraseEntry? currentPhraseEntry = null;
        string? currentPhraseDefinition = null;
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(currentLine) && currentPhraseEntry != null)
            {
                yield return currentPhraseEntry;
                currentPhraseEntry = null;
            }
            else if (currentLine.Contains('#'))
            {
                currentWord = currentLine.Split('#')[1];
            }
            else if (currentLine.StartsWith(PhrasePrefix))
            {
                if (currentPhraseEntry != null)
                {
                    yield return currentPhraseEntry;
                }
                currentPhraseEntry = new PhraseEntry
                {
                    Name = currentLine[PhrasePrefix.Length..],
                    BaseWord = currentWord ?? ""
                };
            }
            else if (PhraseDefinitionRegEx.IsMatch(currentLine))
            {
                currentPhraseDefinition = currentLine;
                currentPhraseEntry?.DefinitionToExamples.Add(currentPhraseDefinition, []);
            }
            else if (currentLine.StartsWith(PhraseExamplePrefix) && currentPhraseDefinition != null)
            {
                currentPhraseEntry?
                    .DefinitionToExamples[currentPhraseDefinition]
                    .Add(currentLine[PhraseExamplePrefix.Length..]);
            }
        }
        if (currentPhraseEntry != null)
        {
            yield return currentPhraseEntry;
        }
    }
}