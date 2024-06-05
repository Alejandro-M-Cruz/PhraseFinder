using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Extensions;
using PhraseFinder.WCF.ServicioLematizacion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhraseFinder.WCF.Models
{
    public class PhrasePattern
    {
        public string Phrase { get; set; }
        public string Pattern { get; set; }
        public string BaseWord { get; set; }
        public int PhraseId { get; set; }

        private static readonly string[] WildcardWords = { "algo", "alguien" };
        private static readonly string[] CliticPronouns = 
        { 
            "se", "te", "le", "les", "lo", "los", "la", "las", "me", "os", "nos"
        };
        private string[] _patternWords;
        
        public IEnumerable<FoundPhrase> FindPhrase(InfoUnaFrase sentence, int sentenceIndexInText)
        {
            if (sentence.Palabras.Count == 0)
            {
                yield break;
            }

            _patternWords = Pattern
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.TrimEnd(','))
                .ToArray();

            if (_patternWords.Length == 0)
            {
                yield break;
            }

            var firstWordInPattern = _patternWords[0];
            var anyWords = 0;

            for (var i = 0; i < sentence.Palabras.Count; i++)
            {
                var word = sentence.Palabras[i];

                if (!word.SameWordAs(firstWordInPattern))
                {
                    continue;
                }

                var wordIndex = i + 1;
                var isMatch = true;

                for (var patternWordIndex = 1; patternWordIndex < _patternWords.Length; patternWordIndex++)
                {
                    var patternWord = _patternWords[patternWordIndex];
                    var tag = GetTag(patternWord);

                    if (tag == null)
                    {
                        var wildcardWordCount = WildcardWordCount(patternWord, wordIndex);

                        if (wildcardWordCount > 0)
                        {
                            if (patternWordIndex == _patternWords.Length - 1)
                            {
                                break;
                            }

                            wordIndex = Math.Max(0, wordIndex - 1);
                            anyWords += 3;
                            patternWordIndex += wildcardWordCount - 1;
                            continue;
                        }
                    }

                    if (wordIndex >= sentence.Palabras.Count)
                    {
                        isMatch = false;
                        break;
                    }

                    var w = sentence.Palabras[wordIndex++];

                    if (w.IsPunctuationMark())
                    {
                        patternWordIndex--;
                        continue;
                    }

                    if (tag?.Type == PatternTagType.AnyWord ||
                        w.SameWordAs(patternWord) || 
                        (tag?.Type == PatternTagType.AnyInflection && w.SameWordAnyInflection(tag.Value)))
                    {
                        continue;
                    }

                    var verbMatchWordCount = VerbMatchWordCount(sentence, wordIndex - 1, verb: patternWord);

                    if (verbMatchWordCount > 0)
                    {
                        wordIndex += verbMatchWordCount - 1;
                        continue;
                    }

                    if (tag?.IsOptional == true)
                    {
                        continue;
                    }

                    if (anyWords < 1 || patternWordIndex == _patternWords.Length - 1)
                    {
                        isMatch = false;
                        break;
                    }

                    anyWords--;
                    patternWordIndex--;
                }

                if (!isMatch)
                {
                    continue;
                }

                var phraseMatch = sentence.SubstringInWordRange(i, wordIndex - i);

                yield return new FoundPhrase
                {
                    PhraseId = PhraseId,
                    Phrase = Phrase,
                    BaseWord = BaseWord,
                    StartIndex = sentenceIndexInText + sentence.IndexOfWord(i),
                    Match = phraseMatch,
                    Length = phraseMatch.Length
                };
            }
        }

        private int WildcardWordCount(string word, int wordIndex)
        {
            if (!IsWildcardWord(word) || _patternWords.Length < 3)
            {
                return 0;
            }

            if (wordIndex > _patternWords.Length - 2 || 
                !_patternWords[wordIndex + 1].EqualsIgnoreCase("o") || 
                !IsWildcardWord(_patternWords[wordIndex + 2]))
            {
                return 1;
            }

            return 3;
        }

        private bool IsWildcardWord(string str)
        {
            return WildcardWords.Any(w => str == w && BaseWord != w);
        }

        private PatternTag GetTag(string str)
        {
            if (str.Length < 3)
            {
                return null;
            }

            switch (str[0])
            {
                case '$':
                    return new PatternTag
                    {
                        Type = PatternTagType.AnyInflection,
                        Value = str.Substring(1)
                    };
                case '<' when str.Last() == '>':
                    return str[1] == '$'
                        ? new PatternTag
                        {
                            Type = PatternTagType.AnyWord,
                            Value = str.Trim('<', '>')
                        }
                        : new PatternTag
                        {
                            Type = PatternTagType.AnyInflection,
                            Value = "haber"
                        };
                case '[' when str.Last() == ']':
                    var tag = GetTag(str.Trim('[', ']'));
                    tag.IsOptional = true;
                    return tag;
            }

            return null;
        }

        private static int VerbMatchWordCount(InfoUnaFrase sentence, int startIndex, string verb)
        {
            for (var i = startIndex; i < sentence.Palabras.Count && i < startIndex + 3; i++)
            {
                var w = sentence.Palabras[i];

                if (w.IsVerb() && w.SameWordAs(verb))
                {
                    return i - startIndex + 1;
                }

                if (w.FormaCanonica != "haber" && CliticPronouns.All(p => !p.EqualsIgnoreCase(w.Palabra)))
                {
                    break;
                }
            }

            return 0;
        }
    }
}