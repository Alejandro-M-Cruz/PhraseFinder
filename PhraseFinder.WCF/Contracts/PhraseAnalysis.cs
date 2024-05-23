using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PhraseFinder.WCF.Contracts
{
    [DataContract]
    public class PhraseAnalysis
    {
        [DataMember] 
        public string ProcessedText { get; set; } = string.Empty;

        [DataMember] 
        public IEnumerable<FoundPhrase> FoundPhrases { get; set; } = Array.Empty<FoundPhrase>();
    }
}