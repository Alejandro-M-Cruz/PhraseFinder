using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;


public class TwoVariantPhraseSplitter : IPhraseSplitter
{
	private static readonly Regex TwoVariantPhraseRegex = new(
		@"^[^,]+, o (?:([^,\?]+?|¿[^,]+\?)|([^,\?]+),([^,\?]+))$",
		RegexOptions.Compiled);

	public string[] SplitPhrase(string phrase)
	{
		var match = TwoVariantPhraseRegex.Match(phrase);

		if (!match.Success)
		{
			return [phrase];
		}

		var firstPart = phrase.Split(", o")[0];
		var secondPart = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;

		if (secondPart.StartsWith('¿'))
		{
			return [firstPart, secondPart];
		}

		var lastPart = match.Groups[3].Value;
		var firstPartWords = firstPart.Split(' ');
		var secondPartWords = secondPart.Split(' ');
		var secondVariant = secondPart;

		if (firstPartWords.Length == 1 ||
		    secondPartWords.First().StartsWith(firstPartWords.First()))
		{
			return [firstPart + lastPart, secondPart + lastPart];
		}

		switch (secondPartWords.Length)
		{
			case 1:
				secondVariant = string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart;
				break;
			case 2:
				for (var i = 0; i < firstPartWords.Length; i++)
				{
					if (firstPartWords[i].Length < 4)
					{
						secondVariant = string.Join(' ', firstPartWords[..i]) + ' ' + secondPart;
						return [firstPart + lastPart, (secondVariant + lastPart).Trim()];
					}
				}
				var wordsToTakeFromFirstPart = secondPartWords[0].Contains("un") ? 1 : 2;
				var secondVariantPrefix = string.Join(' ', firstPartWords[..^wordsToTakeFromFirstPart]);
				secondVariant = secondVariantPrefix + ' ' + secondPart;
				break;
			default:
				for (var i = 0; i < firstPartWords.Length; i++)
				{
					if (firstPartWords[i].Length < 4)
					{
						secondVariant = string.Join(' ', firstPartWords[..i]) + ' ' + secondPart;
						break;
					}
				}
				break;
		}

		return [firstPart + lastPart, (secondVariant + lastPart).Trim()];
	}
}