using System;
using System.Collections.Generic;
using System.Linq;
using PhraseFinder.WCF.Data;

namespace PhraseFinder.WCF
{
    public class PhraseFinderService : IPhraseFinderService
    {
        private static readonly IEnumerable<Phrase> Phrases;

        static PhraseFinderService()
        {
            using (var phrasesService = new PhrasesService())
            {
                Phrases = phrasesService.GetPhrases();
            }
        }

        public IEnumerable<FoundPhrase> FindPhrases(string text)
        {
            var foundPhrases = FindPhrasesInText(text).ToArray();
            IncludeDefinitions(ref foundPhrases);
            return foundPhrases;
	    }

        private IEnumerable<FoundPhrase> FindPhrasesInText(string text)
        {
            foreach (var phrase in Phrases)
            {
                var index = text.IndexOf(phrase.Value, StringComparison.Ordinal);
                if (index != -1)
                {
                    yield return new FoundPhrase
                    {
                        PhraseId = phrase.PhraseId,
                        Phrase = phrase.Value,
                        StartIndex = index,
                        EndIndex = index + phrase.Value.Length,
                        Length = phrase.Value.Length
                    };
                }
            }
        }

        private static void IncludeDefinitions(ref FoundPhrase[] foundPhrases)
        {
            using (var phrasesService = new PhrasesService())
            {
                phrasesService.LoadPhraseDefinitions(foundPhrases.Select(fp => fp.PhraseId));

                foreach (var foundPhrase in foundPhrases)
                {
                    foundPhrase.Definitions = phrasesService.GetPhraseDefinitions(foundPhrase.PhraseId);
                }
            }
        }
    }
}
