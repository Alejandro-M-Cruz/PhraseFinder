using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PhraseFinder.WCF
{
	[DataContract]
	public class FoundPhrase
	{
		[DataMember]
		public int PhraseId { get; set; }

		[DataMember]
		public string Phrase { get; set; } = "";
        
        [DataMember]
		public int StartIndex { get; set; }
		
		[DataMember]
		public int EndIndex { get; set; }
		
		[DataMember]
		public int Length { get; set; }
		
		[DataMember]
        public PhraseDefinition[] Definitions;
    }

	[DataContract]
    public class PhraseDefinition
    {
		[DataMember]
		public string Definition { get; set; } = "";

        [DataMember] 
        public string[] Examples { get; set; }

        [DataMember]
        public int PhraseId { get; set; }
    }
}