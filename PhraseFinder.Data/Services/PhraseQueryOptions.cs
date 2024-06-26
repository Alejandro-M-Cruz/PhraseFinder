﻿using System.ComponentModel.DataAnnotations;
using PhraseFinder.Data.Extensions;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.Data.Services;

public enum PhraseOrderByOption
{
	[Display(Name = "ID ascendente")]
	Id,
	[Display(Name = "ID descendente")]
	IdDesc,
	[Display(Name = "Expresión o locución asc.")]
	Value,
	[Display(Name = "Expresión o locución desc.")]
	ValueDesc,
	[Display(Name = "Palabra base asc.")]
	BaseWord,
    [Display(Name = "Palabra base desc.")]
    BaseWordDesc,
	[Display(Name = "Categorías asc.")]
	Categories,
	[Display(Name = "Categorías desc.")]
    CategoriesDesc,
	[Display(Name = "Revisado asc.")]
	Reviewed,
	[Display(Name = "Revisado desc.")]
	ReviewedDesc
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
	public int TotalPages { get; set; } = 1;

	public void UpdatePagination(IQueryable<Phrase> phrases)
	{
		TotalPages = phrases.GetTotalPages(PageSize);
		Page = Math.Min(Math.Max(1, Page), TotalPages);

        if (CurrentState == _previousState)
        {
            return;
        }

        Page = 1;
        _previousState = CurrentState;
    }
}
