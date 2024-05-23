using System.Runtime.Serialization;
using System;

namespace PhraseFinder.WCF.Contracts
{
    [DataContract]
    public class FoundPhrase
    {
        [DataMember]
        public int PhraseId { get; set; }

        [DataMember]
        public string Phrase { get; set; } = string.Empty;

        [DataMember]
        public string BaseWord { get; set; } = string.Empty;

        [DataMember]
        public int StartIndex { get; set; }

        [DataMember]
        public int EndIndex { get; set; }

        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public PhraseDefinition[] Definitions { get; set; } = Array.Empty<PhraseDefinition>();
    }
}