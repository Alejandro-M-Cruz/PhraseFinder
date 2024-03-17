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

    public string RemoveVide(string phrase)
    {
        return phrase.Split(" V.")[0];
    }

    public string RemoveUnwantedCharacters(string phrase)
    {
        phrase = RemoveSpecification(phrase);
        phrase = RemoveVide(phrase);
        return phrase;
    }
}