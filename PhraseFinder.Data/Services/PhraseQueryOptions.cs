using PhraseFinder.Data.Extensions;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public enum PhraseOrderByOption
{
	Id,
	Value
}

public class PhraseQueryOptions
{
	public string SearchText { get; set; } = string.Empty;

	public const PhraseOrderByOption DefaultOrderByOption = PhraseOrderByOption.Id;
	public PhraseOrderByOption OrderByOption { get; set; } = DefaultOrderByOption;

	private string _previousState = "";

	private string CurrentState => $"{SearchText},{OrderByOption}";

	public const int DefaultPageSize = 20;
	public static int[] PageSizeOptions => [10, 20, 50, 100];

	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = DefaultPageSize;
	public int TotalPages { get; set; } = 0;

	public void UpdatePagination(IQueryable<Phrase> phrases)
	{
		TotalPages = phrases.GetTotalPages(PageSize);
		Page = Math.Min(Math.Max(1, Page), TotalPages);

		if (CurrentState != _previousState)
		{
			Page = 1;
			_previousState = CurrentState;
		}
	}
}
