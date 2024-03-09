using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public interface IPhraseService
{
    public IEnumerable<Phrase> GetPhrases(
        PhraseDictionary phraseDictionary,
        Func<Phrase, object>? orderBy = null,
        int skip = 0,
        int take = 100);
}