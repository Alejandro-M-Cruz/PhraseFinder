﻿using System.Text.RegularExpressions;

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

            return 
            [
                firstPart + (substitutionIndex < 1 ? ' ' + string.Join(' ', secondPartWords.Skip(1)) : "").TrimEnd(),
				(substitutionIndex != -1 ? firstPart[..substitutionIndex] : "") + secondPart
            ];
        }

		if (lastPart.Length == 0)
        {
            var lastMatch = Array.LastIndexOf(firstPartWords, secondPartWords[0]);

            if (lastMatch == 0)
            {
                return
                [
                    firstPart,
                    secondPart
                ];
            }

            if (lastMatch != -1)
            {
                return
                [
                    firstPart,
                    (string.Join(' ', firstPartWords[..lastMatch])) + ' ' + secondPart
                ];
            }

            if (firstPartWords.Length > 2 && secondPartWords.Length > 2 &&
                firstPartWords.TakeLast(secondPartWords.Length - 1).SequenceEqual(secondPartWords.Skip(1)))
            {
                return
                [
                    firstPart,
                    string.Join(' ', firstPartWords.SkipLast(secondPartWords.Length - 1)) + ' ' + secondPart
                ];
            }

            if ((firstPartWords[^1].Length < 3 && secondPartWords[0].Length < 3) ||
                secondPartWords[0].StartsWith(firstPartWords[^1]))
            {
                return
                [
                    firstPart + (secondPartWords.Length > 1 ? ' ' + string.Join(' ', secondPartWords.Skip(1)) : ""),
                    string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart
                ];
            }

            if (secondPartWords[0].Length < 3 && firstPartWords.Length > 1 &&
                firstPartWords[^2].Length > 3 && firstPartWords[^1] == secondPartWords[^1])
            {
                return
                [
                    firstPart,
                    string.Join(' ', firstPartWords[..^1]) + ' ' + secondPart
                ];
            }
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