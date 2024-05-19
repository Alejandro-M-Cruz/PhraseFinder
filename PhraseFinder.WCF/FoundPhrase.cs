using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PhraseFinder.WCF
{
	[DataContract]
	public class FoundPhrase
	{
		[DataMember]
		public string Phrase { get; set; } = "";
        
        [DataMember]
		public int StartIndex { get; set; }
		
		[DataMember]
		public int EndIndex { get; set; }
		
		[DataMember]
		public int Length { get; set; }
		
		[DataMember]
		public IDictionary<string, string[]> DefinitionToExamples { get; } =
			new Dictionary<string, string[]>();
	}
}