using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services;

public interface IPhraseDictionaryReader
{
    public IEnumerable<PhraseEntry> ReadPhraseEntries();
    public IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync();
}