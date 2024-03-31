using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.FileReaders;

public interface IPhraseDictionaryFileReader
{
    public IAsyncEnumerable<PhraseEntry> ReadPhraseEntriesAsync(
	    CancellationToken cancellationToken = default);
}