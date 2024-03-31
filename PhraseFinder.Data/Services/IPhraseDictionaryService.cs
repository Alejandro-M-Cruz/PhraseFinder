using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public interface IPhraseDictionaryService
{
    public Task<IEnumerable<PhraseDictionary>> GetPhraseDictionariesAsync();
    public Task AddPhraseDictionaryAsync(PhraseDictionary phraseDictionary);
    public Task AddPhraseDictionaryFromFileAsync(
	    PhraseDictionary phraseDictionary,
	    CancellationToken cancellationToken = default);
    public Task UpdatePhraseDictionaryAsync(PhraseDictionary phraseDictionary);
    public Task DeletePhraseDictionaryAsync(PhraseDictionary phraseDictionary);
}