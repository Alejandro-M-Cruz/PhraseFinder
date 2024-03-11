using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public class PhraseService(PhraseFinderDbContext dbContext) : IPhraseService
{
    public IEnumerable<Phrase> GetPhrases(PhraseDictionary phraseDictionary) => 
        dbContext.Phrases.Where(p => p.PhraseDictionaryId == phraseDictionary.PhraseDictionaryId);
}