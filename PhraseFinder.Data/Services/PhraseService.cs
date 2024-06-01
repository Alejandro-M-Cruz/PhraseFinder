using Microsoft.EntityFrameworkCore;
using PhraseFinder.Data.Extensions;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public class PhraseService(PhraseFinderDbContext dbContext) : IPhraseService
{
    public IQueryable<Phrase> GetPhrases(
	    PhraseDictionary phraseDictionary, 
	    PhraseQueryOptions options) 
    {
        var phrases = dbContext.Phrases
	        .AsNoTracking()
	        .Where(p => p.PhraseDictionaryId == phraseDictionary.PhraseDictionaryId)
	        .SearchPhrasesBy(options.SearchText)
	        .OrderPhrasesBy(options.OrderByOption);
		options.UpdatePagination(phrases);
		return phrases.Paginate(options.Page, options.PageSize);
	}
}
