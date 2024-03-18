using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services;

public class PhraseGenderSplitter
{
    private static readonly Regex PhraseWithGenderRegex = new(
        @"\b\w*(\w)(o(s|)),\s\w?\1(a\3)\b|\b\w*([a-kA-Km-zM-Z])(e(s|)),\s\w?\5(a\7)\b|\b\w+(o([a-rA-Rt-zT-Z])),\s(\10(a))\b", 
        RegexOptions.Compiled);

    public string[] SplitPhrase(string phrase)
    {
        var matches = PhraseWithGenderRegex.Matches(phrase);

        if (matches.Count == 0)
        {
            return [phrase];
        }

        var masculineVariant = "";
        var feminineVariant = "";
        var prevMatchEndIndex = 0;

        foreach (var match in matches.AsEnumerable())
        {
            GetSuffixGroups(match.Groups, out var masculineSuffixGroup, out var feminineSuffixGroup);
            var newPart = phrase.Substring(
                prevMatchEndIndex, 
                (masculineSuffixGroup?.Index ?? feminineSuffixGroup.Index - 3) - prevMatchEndIndex);
            masculineVariant += newPart + masculineSuffixGroup?.Value;
            feminineVariant += newPart + feminineSuffixGroup.Value;
            prevMatchEndIndex = feminineSuffixGroup.Index + feminineSuffixGroup.Length;
        }

        var lastPart = phrase.Length > prevMatchEndIndex ? phrase[prevMatchEndIndex..] : "";
        return [masculineVariant + lastPart, feminineVariant + lastPart];
    }

    private static void GetSuffixGroups(
        GroupCollection groups,
        out Group? masculineSuffixGroup,
        out Group feminineSuffixGroup)
    {
        if (groups[2].Success)
        {
            masculineSuffixGroup = groups[2];
            feminineSuffixGroup = groups[4];
        }
        else if (groups[5].Success)
        {
            masculineSuffixGroup = groups[6];
            feminineSuffixGroup = groups[8];
        }
        else
        {
            masculineSuffixGroup = null;
            feminineSuffixGroup = groups[12];
        }
    }
}