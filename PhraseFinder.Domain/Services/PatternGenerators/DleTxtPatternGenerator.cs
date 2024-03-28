using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services.PhraseCleaners;
using PhraseFinder.Domain.Services.PhraseSplitters;

namespace PhraseFinder.Domain.Services.PatternGenerators;

public class DleTxtPatternGenerator(
    IPhraseCleaner phraseCleaner,
    IPhraseSplitter[] phraseSplitters) : IPatternGenerator
{
    public IEnumerable<Phrase> GeneratePatterns(Phrase phrase)
    {
        var cleanPhrase = phraseCleaner.CleanPhrase(phrase.Value);
        return ApplySplitters(cleanPhrase).Select(split => phrase with
        {
            Value = cleanPhrase,
            Variant = split,
            Pattern = split,
            Reviewed = !cleanPhrase.Contains("Tb.") && !cleanPhrase.Contains(", "),
        });
    }

    private IEnumerable<string> ApplySplitters(string phrase)
    {
        if (phraseSplitters.Length == 0)
        {
            return [phrase];
        }

        return phraseSplitters
            .Skip(1)
            .Aggregate(
                phraseSplitters[0].SplitPhrase(phrase).AsEnumerable(), 
                (current, splitter) => current.SelectMany(splitter.SplitPhrase));
    }
}