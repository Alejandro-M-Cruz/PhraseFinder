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
		var firstPartWords = firstPart.Split();
		var secondPartWords = secondPart.Split();

        if (secondPartWords.Length == 0 || string.IsNullOrWhiteSpace(secondPart))
        {
            return [firstPart + lastPart];
        }

        if (firstPartWords.Length == 1 || 
            (firstPart.Contains("a la") && secondPartWords.First() == "al"))
        {
            if (lastPart.Length > 0)
            {
				return [firstPart + lastPart, secondPart + lastPart];
            }

            var substitutionIndex = firstPart.LastIndexOf("a la", StringComparison.Ordinal);

            return [
                firstPart + (substitutionIndex < 1 ? ' ' + string.Join(' ', secondPartWords.Skip(1)) : ""),
				(substitutionIndex != -1 ? firstPart[..substitutionIndex] : "") + secondPart
            ];
        }

		if (lastPart.Length == 0 && 
            firstPartWords.Last().Length < 3 && 
            secondPartWords.First().Length < 3)
        {
            return [
				firstPart + ' ' + string.Join(' ', secondPartWords.Skip(1)),
				string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart
            ];
        }

		if (secondPartWords.First().StartsWith(firstPartWords.First()))
		{
			return [firstPart + lastPart, secondPart + lastPart];
		}

        string secondVariant;

        switch (secondPartWords.Length)
		{
			case 1:
				secondVariant = string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart;
				break;
			default:
				for (var i = 0; i < firstPartWords.Length; i++)
                {
                    if (firstPartWords[i].Length > 3)
                    {
                        continue;
                    }
                    secondVariant = string.Join(' ', firstPartWords[..i]) + ' ' + secondPart;
                    return [firstPart + lastPart, (secondVariant + lastPart).Trim()];
                }
				var longWordCount = secondPartWords.Count(w => w.Length > 3);
				var wordsToDiscardFromFirstPart = Math.Min(longWordCount, firstPartWords.Length);
				var secondVariantPrefix = string.Join(' ', firstPartWords[..^wordsToDiscardFromFirstPart]);
				secondVariant = secondVariantPrefix + ' ' + secondPart;
				break;
		}

		return [firstPart + lastPart, (secondVariant + lastPart).Trim()];
	}
}