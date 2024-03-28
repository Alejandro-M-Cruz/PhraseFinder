using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public interface IPhraseService
{
    public IQueryable<Phrase> GetPhrases(
	    PhraseDictionary phraseDictionary, 
	    PhraseQueryOptions options);
}