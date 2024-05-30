namespace PhraseFinder.WCF.Models
{
    public class PhrasePattern
    {
        public string Phrase { get; set; }
        public string Variant { get; set; }
        public string Pattern { get; set; }
        public string BaseWord { get; set; }
        public int PhraseId { get; set; }
    }
}