using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public interface IPhraseDictionaryService
{
    public Task AddPhraseDictionaryAsync(string filePath);
    public Task AddPhraseDictionaryAsync(PhraseDictionary phraseDictionary);
    public Task RemovePhraseDictionaryAsync(PhraseDictionary phraseDictionary);
}