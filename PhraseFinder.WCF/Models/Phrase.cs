namespace PhraseFinder.WCF.Models
{
    public class Phrase
    {
        public int PhraseId { get; set; }
        public string Value { get; set; }
        public string Pattern { get; set; }
        public string BaseWord { get; set; }
    }
}