namespace PhraseFinder.WCF.Models
{
    public class PatternTag
    {
        public PatternTagType Type { get; set; }
        public string Value { get; set; }
        public bool IsOptional { get; set; } = false;
    }
}