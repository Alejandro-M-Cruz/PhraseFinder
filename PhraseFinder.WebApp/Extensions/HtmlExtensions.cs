using System.Text;
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

		try
		{
			foreach (var phrase in foundPhrases)
			{
				n++;

				if (phrase.StartIndex > currentIndex)
				{
					highlightedText.Append(text[currentIndex..phrase.StartIndex]);
				}
				else if (currentIndex != 0)
				{
					highlightedText.Append("<span> | </span>");
				}

				highlightedText.Append(
					$"<a id=\"phrase-link-{n}\" href=\"#phrase-{n}\" class=\"text-danger text-decoration-none\" " +
					$"onclick=\"focusPhrase({n})\"><strong class=\"text-decoration-underline fw-bold\">");
				highlightedText.Append(text, phrase.StartIndex, phrase.Length);
				highlightedText.Append($"</strong><sup>{n}</sup></a>");

				currentIndex = phrase.EndIndex;
			}

			if (currentIndex < text.Length)
			{
				highlightedText.Append(text[currentIndex..]);
			}
		}
		catch
		{
			highlightedText.Clear();
			highlightedText.Append(text);
		}

		return helper.Raw(highlightedText);
	}
}