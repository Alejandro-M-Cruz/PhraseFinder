using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseCleaners;

public class DleTxtPhraseCleaner : IPhraseCleaner
{
    private static readonly Regex ExtraSectionsRegex = new(
        @"(?: U\.| V\.| Por alus\.| La var\.| En acep\.| Del| Expr\.| Escr\.| De| Falsa| Quizá) .+\.",
        RegexOptions.Compiled);

    private static readonly Regex SpecificationRegex = new(
        @",\sespecialmente.*?,|,\sespecialmente.*?$",
        RegexOptions.Compiled);

    public string CleanPhrase(string phrase)
    {
        phrase = RemoveSpecification(phrase);
        return RemoveExtraSections(phrase);
    }

    private string RemoveSpecification(string phrase)
    {
        var match = SpecificationRegex.Match(phrase);
        return match.Success ? phrase.Replace(match.Value, "") : phrase;
    }

    private string RemoveExtraSections(string phrase)
    {
        var match = ExtraSectionsRegex.Match(phrase);
        return match.Success ? phrase.Remove(match.Index) : phrase;
    }
}