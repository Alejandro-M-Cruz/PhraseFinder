using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.FileReaders;

public interface IPhraseDictionaryFileReader
{
    public IEnumerable<PhraseEntry> ReadPhraseEntries();
    public IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync();
}