using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public interface IPhraseService
{
    public IEnumerable<Phrase> GetPhrases(PhraseDictionary phraseDictionary);
}