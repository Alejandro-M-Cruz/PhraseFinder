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
        public int Length { get; set; }

        [DataMember]
        public string Match { get; set; } = string.Empty;

        [DataMember]
        public PhraseDefinition[] Definitions { get; set; } = Array.Empty<PhraseDefinition>();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((FoundPhrase)obj);
        }

        protected bool Equals(FoundPhrase other)
        {
            return PhraseId == other.PhraseId && StartIndex == other.StartIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PhraseId * 397) ^ StartIndex;
            }
        }
    }

    //[DataContract]
    //public class FoundPhraseMatch
    //{
    //    [DataMember]
    //    public string Pattern { get; set; } = string.Empty;

    //    [DataMember]
    //    public string Value { get; set; } = string.Empty;

    //    [DataMember]
    //    public int Index { get; set; }

    //    [DataMember]
    //    public int Length { get; set; }
    //}
}