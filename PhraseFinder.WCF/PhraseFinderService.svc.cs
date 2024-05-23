using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Data;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.ServicioLematizacion;

namespace PhraseFinder.WCF
{
    public class PhraseFinderService : IPhraseFinderService
    {
        private static readonly IEnumerable<Phrase> Phrases;
        private static readonly ServicioLematizacionClient ServicioLematizacion = 
            new ServicioLematizacionClient("BasicHttpsBinding_IServicioLematizacion");

        static PhraseFinderService()
        {
            using (var phrasesService = new PhrasesService())
            {
                Phrases = phrasesService.GetPhrases();
            }
        }

        public async Task<PhraseAnalysis> FindPhrasesAsync(string text)
        {
            var sentences = await ServicioLematizacion.NuevoReconocerFrasesAsync(
                text.GetSentences().ToList(), 
                idioma: "es", 
                multiPref: false);
            var foundPhrases = FindPhrasesInSentences(sentences).ToArray();
            IncludeDefinitions(ref foundPhrases);
            return new PhraseAnalysis
            {
                ProcessedText = string.Join(" ", sentences.Select(s => s.Frase)),
                FoundPhrases = foundPhrases
            };
	    }

        private IEnumerable<FoundPhrase> FindPhrasesInSentences(IReadOnlyList<InfoUnaFrase> sentences)
        {
            var foundPhrases = new List<FoundPhrase>();

            foreach (var phrase in Phrases)
            {
                var phraseWords = phrase.Value.Split(' ').Select(w => w.TrimEnd(',')).ToArray();
                var firstWordInPhrase = phraseWords.First();
                var sentenceIndex = 0;

                foreach (var sentence in sentences)
                {
                    foreach (var word in sentence.Palabras)
                    {
                        if (!word.CoincidesWith(firstWordInPhrase))
                        {
                            continue;
                        }

                        var pos = word.Posicion;

                        if (!phraseWords.Skip(1).All(pw =>
                            {
                                var w = sentence.Palabras[pos];
                                if (w.Posicion == 0)
                                {
                                    return true;
                                }
                                pos++;
                                return w.CoincidesWith(pw);
                            }))
                        {
                            continue;
                        }

                        foundPhrases.Add(new FoundPhrase
                        {
                            PhraseId = phrase.PhraseId,
                            Phrase = phrase.Value,
                            BaseWord = phrase.BaseWord,
                            StartIndex = sentenceIndex + sentence.IndexOfWordInPosition(pos),
                            Length = 1
                        });
                    }
                    sentenceIndex += sentence.Frase.Length + 1;
                }
            }

            return foundPhrases;
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
