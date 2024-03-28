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
		var secondVariant = "";

		Console.WriteLine(lastPart);

		if (firstPartWords.Length == 1)
		{
			return [firstPart + lastPart, secondPart + lastPart];
		}

		if (firstPartWords.FirstOrDefault() == secondPartWords.FirstOrDefault())
		{
			secondVariant = string.Join(' ', secondPartWords);
			return [firstPart + lastPart, secondVariant + lastPart];
		}

		switch (secondPartWords.Length)
		{
			case 1:
				secondVariant = string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart;
				break;
			case 2:
				var wordsToTakeFromFirstPart = secondPartWords[0].Contains("un") ? 1 : 2;
				var secondVariantPrefix = string.Join(' ', firstPartWords[..^wordsToTakeFromFirstPart]);
				secondVariant = secondVariantPrefix + ' ' + secondPart;
				break;
		}

		return [firstPart + lastPart, (secondVariant + lastPart).Trim()];
	}
}