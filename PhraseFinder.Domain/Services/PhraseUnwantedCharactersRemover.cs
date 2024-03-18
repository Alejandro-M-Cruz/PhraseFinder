using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services;

public partial class PhraseUnwantedCharactersRemover
{
    [GeneratedRegex(@",\sespecialmente.*?,|,\sespecialmente.*?$")]
    private static partial Regex SpecificationRegex();

    public string RemoveSpecification(string phrase)
    {
        var match = SpecificationRegex().Match(phrase);
        return match.Success ? phrase.Replace(match.Value, "") : phrase;
    }

    public string RemoveVideSection(string phrase)
    {
        return phrase.Split(" V.")[0];
    }

    public string RemoveByAllusionSection(string phrase)
    {
        return phrase.Split(" Por alus.")[0];
    }

    public string RemoveUnwantedCharacters(string phrase)
    {
        phrase = RemoveSpecification(phrase);
        phrase = RemoveVideSection(phrase);
        phrase = RemoveByAllusionSection(phrase);
        return phrase;
    }
}