using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services;

public interface IPhraseDictionaryFileReader
{
    public IEnumerable<PhraseEntry> ReadPhraseEntries();
    public IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync();
}