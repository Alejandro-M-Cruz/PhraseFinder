using System.ComponentModel;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Extensions;

public static class PhraseQueryExtensions
{
	
	public static IQueryable<Phrase> SearchPhrasesBy(this IQueryable<Phrase> phrases, string search)
    {
        return string.IsNullOrWhiteSpace(search) ? 
            phrases : 
            phrases.Where(p => p.Value.Contains(search));
    }

	public static IQueryable<Phrase> OrderPhrasesBy(
		this IQueryable<Phrase> phrases, 
		PhraseOrderByOption orderByOption)
	{
		return orderByOption switch
		{
			PhraseOrderByOption.Id => phrases.OrderBy(p => p.PhraseId),
			PhraseOrderByOption.Value => phrases.OrderBy(p => p.Value),
			PhraseOrderByOption.IdDesc => phrases.OrderByDescending(p => p.PhraseId),
			PhraseOrderByOption.ValueDesc => phrases.OrderByDescending(p => p.Value),
			PhraseOrderByOption.BaseWord => phrases.OrderBy(p => p.BaseWord),
			PhraseOrderByOption.BaseWordDesc => phrases.OrderByDescending(p => p.BaseWord),
			PhraseOrderByOption.Categories => phrases.OrderBy(p => p.Categories),
			PhraseOrderByOption.CategoriesDesc => phrases.OrderByDescending(p => p.Categories),
			PhraseOrderByOption.Reviewed => phrases.OrderBy(p => p.Reviewed),
			PhraseOrderByOption.ReviewedDesc => phrases.OrderByDescending(p => p.Reviewed),
			_ => throw new InvalidEnumArgumentException(
				nameof(orderByOption), 
				(int)orderByOption, 
				typeof(PhraseOrderByOption))
		};
	}
}
