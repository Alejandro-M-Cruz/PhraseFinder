using System.Diagnostics;
using System.Net.NetworkInformation;

namespace PhraseFinder.Domain.Services;

public class PhraseSplitter
{
    private static readonly IDictionary<int, int> WordsToOverwrite = new Dictionary<int, int>
    {
        {2, 1}, {3, 2}, {4, 3}
    };

    public string[] SplitPhrase(string phrase)
    {
        if (!phrase.Contains(", o "))
        {
            return [phrase];
        }
        var parts = phrase.Trim().Split(", o ");
        return parts.Length switch
        {
            2 => SplitPhraseWithTwoVariants(parts),
            3 => SplitPhraseWithThreeVariants(parts),
            _ => throw new InvalidOperationException("Phrase has more than 3 variants")
        };
    }

    private string[] SplitPhraseWithTwoVariants(string[] parts)
    {
        var firstVariant = parts[0];
        var firstVariantWords = firstVariant.Split(' ');
        var secondPart = parts[1];
        var suffix = secondPart;
        if (secondPart.Contains(", "))
        {
            var secondPartFragments = secondPart.Split(", ");
            suffix = string.Join(' ', secondPartFragments);
            firstVariant += " " + secondPartFragments[1];
        }
        int wordsToReplace = DetermineWordsToReplace(firstVariantWords, suffix);

        var prefix = string.Join(' ', firstVariantWords.Take(firstVariantWords.Length - wordsToReplace));
        var secondVariant = string.IsNullOrWhiteSpace(prefix) ? suffix : prefix + " " + suffix;
        return [firstVariant, secondVariant];
    }

    private string[] SplitPhraseWithThreeVariants(string[] parts)
    {
        throw new NotImplementedException();
    }

    private int DetermineWordsToReplace(string[] firstVariantWords, string suffix)
    {
        var suffixWords = suffix.Split(' ');
        var lastFirstVariantWord = firstVariantWords.Last();

        if (suffixWords.Length == 1)
        {
            return 1;
        }

        for (var i = 0; i < firstVariantWords.Length; i++)
        {
            if (firstVariantWords[i] == suffixWords.First())
            {
                return firstVariantWords.Length - i;
            }
        }

        for (var i = 0; i < suffixWords.Length; i++)
        {
            if (suffixWords[i].Contains(lastFirstVariantWord))
            {
                return i;
            }
        }

        if (suffixWords.Length == 3)
        {
            return firstVariantWords.Length > 2 ? 2 : 1;
        }

        return 1;
    }
}