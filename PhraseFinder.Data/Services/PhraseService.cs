using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public class PhraseService(PhraseFinderDbContext dbContext) : IPhraseService
{
    public IEnumerable<Phrase>? GetPhrasesAsync(PhraseDictionary phraseDictionary)
    {
        return dbContext.Entry(phraseDictionary)
            .Collection(p => p.Phrases)
            .CurrentValue;
    }
}