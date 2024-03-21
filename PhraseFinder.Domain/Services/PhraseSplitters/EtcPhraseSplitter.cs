using System.Text.RegularExpressions;

namespace PhraseFinder.Domain.Services.PhraseSplitters;

public class EtcPhraseSplitter : IPhraseSplitter
{
    private static readonly Regex PhraseWithEtcRegex = new(
        @"",
        RegexOptions.Compiled);


    public string[] SplitPhrase(string phrase)
    {
        throw new NotImplementedException();
    }
}