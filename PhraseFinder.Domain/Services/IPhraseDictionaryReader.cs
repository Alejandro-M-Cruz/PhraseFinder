using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services;

public interface IPhraseDictionaryReader
{
    public IAsyncEnumerable<Phrase> ReadPhraseEntriesAsync();
    public IEnumerable<Phrase> ReadPhraseEntries();
}