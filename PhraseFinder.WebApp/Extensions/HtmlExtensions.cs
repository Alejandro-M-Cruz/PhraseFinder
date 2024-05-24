using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Extensions;

public static class HtmlExtensions
{
	public static IHtmlContent HighlightPhrases(this IHtmlHelper htmlHelper, string text, FoundPhrase[] foundPhrases)
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
					highlightedText.Append(htmlHelper.Encode(text[currentIndex..phrase.StartIndex]));
				}
				else if (currentIndex != 0)
				{
					highlightedText.Append("<span> | </span>");
				}

				highlightedText.Append(
					$"<a id=\"phrase-link-{n}\" href=\"#phrase-{n}\" class=\"text-danger text-decoration-none\">" +
					$"<strong class=\"text-decoration-underline fw-bold\">");
                var phraseToHighlight = text.Substring(phrase.StartIndex, phrase.Length);
                highlightedText.Append(htmlHelper.Encode(phraseToHighlight));
				highlightedText.Append($"</strong><sup>{n}</sup></a>");

				currentIndex = phrase.StartIndex + phrase.Length; 
			}

			if (currentIndex < text.Length)
			{
				highlightedText.Append(htmlHelper.Encode(text[currentIndex..]));
			}
		}
		catch
		{
			highlightedText.Clear();
			highlightedText.Append(htmlHelper.Encode(text));
		}

		return htmlHelper.Raw(highlightedText);
	}
}