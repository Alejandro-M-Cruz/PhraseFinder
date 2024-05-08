﻿using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Extensions;

public static class HtmlExtensions
{
	public static IHtmlContent HighlightPhrases(this IHtmlHelper helper, string text, FoundPhrase[] foundPhrases)
	{
		var n = 0;
		var currentIndex = 0;
		var highlightedText = new StringBuilder();

		foreach (var phrase in foundPhrases)
		{
			n++;

			if (phrase.StartIndex > currentIndex)
			{
				highlightedText.Append($"<span>{text[currentIndex..phrase.StartIndex]}</span>");
			} 
			else if (currentIndex != 0)
			{
				highlightedText.Append("<span> | </span>");
			}

			highlightedText.Append($"<a href=\"#phrase-{n}\" class=\"text-danger fw-bold\">");
			highlightedText.Append(text, phrase.StartIndex, phrase.Length);
			highlightedText.Append($"</a><sup class=\"text-danger fw-bold\">{n}</sup>");

			currentIndex = phrase.EndIndex;
		}

		return helper.Raw(highlightedText);
	}
}