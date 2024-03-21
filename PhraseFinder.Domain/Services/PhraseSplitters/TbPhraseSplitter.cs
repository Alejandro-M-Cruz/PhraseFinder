using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;

public class TbPhraseSplitter : IPhraseSplitter
{
    private static readonly Regex SpecificationInAlsoSectionRegex = new(
        @" Tb\. .+(, \w+\.)",
        RegexOptions.Compiled);

    public string[] SplitPhrase(string phrase)
    {
        if (!phrase.Contains("Tb."))
        {
            return [phrase];
        }

        var match = SpecificationInAlsoSectionRegex.Match(phrase);
        var specificationGroup = match.Groups[1];

        if (specificationGroup.Success)
        {
            phrase = phrase.Remove(specificationGroup.Index);
        }

        var parts = phrase.Split(" Tb. ");
        var otherVariants = parts[1].Split('.')[0].Split("; ");
        return [parts[0], .. otherVariants];
    }
}