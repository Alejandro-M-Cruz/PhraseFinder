using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.PhraseDictionaryReaders;

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
public class DleTxtPhraseDictionaryReader(string filePath) : IPhraseDictionaryReader
{
    private const string PhraseTag = "[loc6]";
    private const string PhraseExampleTag = "[Ejem]";

    public static readonly Regex EntryRegex = new(@"^.+#\w+", RegexOptions.Compiled);
    public static readonly Regex PhraseDefinitionRegex = new(
        @"^\d+\. ((?:loc\.|locs\.|expr\.|exprs\.) (?:[a-z]+\. )*)",
        RegexOptions.Compiled);
    private static readonly Regex EntryNumberRegex = new(@"\[\d+\]", RegexOptions.Compiled);

    public async IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(filePath);
        string? currentLine;
        string? currentWord = null;
        PhraseEntry? currentPhraseEntry = null;
        string? currentPhraseDefinition = null;
        Match phraseDefinitionMatch;

        while ((currentLine = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            if (string.IsNullOrWhiteSpace(currentLine) && currentPhraseEntry != null)
            {
                if (currentPhraseEntry.Categories.Count > 0)
                {
                    yield return currentPhraseEntry;
                }
                currentPhraseEntry = null;
                currentPhraseDefinition = null;
            }
            else if (EntryRegex.IsMatch(currentLine))
            {
                var entry = currentLine.Split('#').Last();
                var entryNumberMatch = EntryNumberRegex.Match(entry);
                currentWord = entryNumberMatch.Success ?
                    entry.Remove(entryNumberMatch.Index, entryNumberMatch.Length) :
                    entry;
                currentPhraseDefinition = null;
            }
            else if (currentLine.StartsWith(PhraseTag))
            {
                if (currentPhraseEntry?.Categories.Count > 0)
                {
                    yield return currentPhraseEntry;
                }
                currentPhraseEntry = new PhraseEntry
                {
                    Name = currentLine[PhraseTag.Length..],
                    BaseWord = currentWord ?? ""
                };
                currentPhraseDefinition = null;
            }
            else if (
                (phraseDefinitionMatch = PhraseDefinitionRegex.Match(currentLine)).Success && 
                currentPhraseEntry != null)
            {
                currentPhraseDefinition = currentLine;
                currentPhraseEntry.DefinitionToExamples.Add(currentPhraseDefinition, []);
                currentPhraseEntry.Categories.Add(phraseDefinitionMatch.Groups[1].Value.TrimEnd());
            }
            else if (currentLine.StartsWith(PhraseExampleTag) && currentPhraseDefinition != null)
            {
                currentPhraseEntry?
                    .DefinitionToExamples[currentPhraseDefinition]
                    .Add(currentLine[PhraseExampleTag.Length..]);
            }
        }
        if (currentPhraseEntry?.Categories.Count > 0)
        {
            yield return currentPhraseEntry;
        }
    }
}