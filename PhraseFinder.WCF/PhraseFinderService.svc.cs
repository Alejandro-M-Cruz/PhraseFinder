using System.Collections.Generic;
using PhraseFinder.WCF.Data;

namespace PhraseFinder.WCF
{
    public class PhraseFinderService : IPhraseFinderService
    {
	    public IEnumerable<FoundPhrase> FindPhrases(string text)
	    {
            using (var phrasesService = new PhrasesService())
            {
                var phrases = phrasesService.GetPhrases();
                var n = 0;
                Phrase lastPhrase = null;
                foreach (var phrase in phrases)
                {
                    n++;
                    lastPhrase = phrase;
                }

                return new[]
                {
                    new FoundPhrase
                    {
                        Phrase = "alguno, na que otro, tra",
                        StartIndex = 0,
                        EndIndex = 10,
                        Length = 10
                    },
                    new FoundPhrase
                    {
                        Phrase = "alguno, na que otro, tra",
                        StartIndex = 33,
                        EndIndex = 39,
                        Length = 6,
                    },
                    new FoundPhrase
                    {
                        Phrase = "texto de ejemplo",
                        StartIndex = 44,
                        EndIndex = 55,
                        Length = 11,
                        DefinitionToExamples =
                        {
                            { "totalPhrases", new[] { n.ToString() } },
                            { "lastPhrase", new[] { lastPhrase?.PhraseId.ToString() ?? "none" } }
                        }
                    }
                };
            }
	    }
    }
}
