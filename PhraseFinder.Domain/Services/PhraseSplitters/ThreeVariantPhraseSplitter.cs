using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;

public class ThreeVariantPhraseSplitter : IPhraseSplitter
{
    private static readonly Regex ThreeVariantPhraseRegex = new (
        "^([^,]+), (?:(?!o ))([^,]+), o ([^,]+)$",
        RegexOptions.Compiled);

    public string[] SplitPhrase(string phrase)
    {
        var match = ThreeVariantPhraseRegex.Match(phrase);

        if (!match.Success)
        {
            return [phrase];
        }

        var firstPart = match.Groups[1].Value;
        var secondPart = match.Groups[2].Value;
        var thirdPart = match.Groups[3].Value;
        var firstPartWords = firstPart.Split();
        var secondPartWords = secondPart.Split();
        var thirdPartWords = thirdPart.Split();

        if (secondPartWords[0] == thirdPartWords[0])
        {
            var index = firstPart.LastIndexOf(secondPartWords[0], StringComparison.Ordinal);

            if (index != -1)
            {
                return [firstPart, firstPart[..index] + secondPart, firstPart[..index] + thirdPart];
            }
        }

        if (secondPartWords.Length < firstPartWords.Length - 1)
        {
            return
            [
                firstPart,
                string.Join(' ', firstPartWords.SkipLast(secondPartWords.Length)) + ' ' + secondPart,
                string.Join(' ', firstPartWords.SkipLast(thirdPartWords.Length)) + ' ' + thirdPart
            ];
        }

        if (thirdPartWords.Length > firstPartWords.Length + 1)
        {
            return 
            [
                firstPart + ' ' + string.Join(' ', thirdPartWords[firstPartWords.Length..]),
                secondPart + ' ' + string.Join(' ', thirdPartWords[secondPartWords.Length..]),
                thirdPart
            ];
        }

        return [firstPart, secondPart, thirdPart];
    }
}