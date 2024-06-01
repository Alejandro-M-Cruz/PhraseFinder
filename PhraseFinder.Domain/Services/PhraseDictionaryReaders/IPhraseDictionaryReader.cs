using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.PhraseDictionaryReaders;

public interface IPhraseDictionaryReader
{
    public IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync(
        CancellationToken cancellationToken = default);
}