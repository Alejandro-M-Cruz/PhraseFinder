using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services;

public class PhraseGenderProcessor
{
    private static readonly Regex GenderRegex = new(
        @"\w+(o|os|e|es),\s(\w{1,2})(a|as)", RegexOptions.Compiled);

    public string[] ProcessPhrase(string phrase)
    {
        //Given the first string, should return a string array of the second and third strings
        //alguno, na que otro, tra",
        //"alguno que otro", "alguna que otra"
        //

        var matches = GenderRegex.Matches(phrase);

        if (matches.Count == 0)
        {
            return [phrase];
        }

        string? masculineVariant = null;
        string? feminineVariant = null;

        var match = matches[0];
        var feminineIndex = match.Groups[2].Index;
        var feminineSuffix = match.Groups[3].Value;
        var feminineSuffixIndex = match.Groups[3].Index;
        var masculineSuffixIndex = match.Groups[1].Index;

        var masculineVariantFirstPart = phrase.Substring(0, feminineIndex - 2);
        masculineVariant =
            masculineVariantFirstPart +
            phrase.Substring(feminineSuffixIndex + feminineSuffix.Length);

        var feminineVariantFirstPart = phrase.Substring(0, masculineSuffixIndex);
        feminineVariant =
            feminineVariantFirstPart +
            phrase.Substring(feminineSuffixIndex);

        return [masculineVariant ?? phrase, feminineVariant ?? phrase];
    }
}