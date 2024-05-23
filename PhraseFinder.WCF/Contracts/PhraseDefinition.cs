using System.Runtime.Serialization;
using System;

namespace PhraseFinder.WCF.Contracts
{
    [DataContract]
    public class PhraseDefinition
    {
        [DataMember]
        public string Definition { get; set; } = string.Empty;

        [DataMember]
        public string[] Examples { get; set; } = Array.Empty<string>();

        [DataMember]
        public int PhraseId { get; set; }
    }
}