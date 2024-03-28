using System.ComponentModel;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Extensions;

public static class PhraseQueryExtensions
{
	
	public static IQueryable<Phrase> SearchPhrasesBy(this IQueryable<Phrase> phrases, string search)
	{
		if (string.IsNullOrWhiteSpace(search))
		{
			return phrases;
		}
		return phrases.Where(p => p.Variant.Contains(search));
	}

	public static IQueryable<Phrase> OrderPhrasesBy(
		this IQueryable<Phrase> phrases, 
		PhraseOrderByOption orderByOption)
	{
		return orderByOption switch
		{
			PhraseOrderByOption.Id => phrases.OrderBy(p => p.PhraseId),
			PhraseOrderByOption.Value => phrases.OrderBy(p => p.Value),
			_ => throw new InvalidEnumArgumentException(
				nameof(orderByOption), 
				(int)orderByOption, 
				typeof(PhraseOrderByOption))
		};
	}
}
