using Microsoft.EntityFrameworkCore;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public class PhraseService(DbContext dbContext) : IPhraseService
{
    public IEnumerable<Phrase>? GetPhrasesAsync(PhraseDictionary phraseDictionary)
    {
        return dbContext.Entry(phraseDictionary)
            .Collection(p => p.Phrases)
            .CurrentValue;
    }
}