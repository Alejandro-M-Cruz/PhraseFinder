using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public class PhraseService(PhraseFinderDbContext dbContext) : IPhraseService
{
    public IEnumerable<Phrase> GetPhrases(
        PhraseDictionary phraseDictionary,
        Func<Phrase, object>? orderBy = null,
        int skip = 0,
        int take = 100)
    {
        return dbContext.Entry(phraseDictionary)
            .Collection(p => p.Phrases)
            .Query()
            .AsEnumerable()
            .OrderBy(orderBy ?? (p => p.PhraseId))
            .Skip(skip)
            .Take(take)
            .ToList();
    }
}