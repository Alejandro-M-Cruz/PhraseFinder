namespace PhraseFinder.Domain.Models;

public class PhraseExample
{
    public int PhraseExampleId { get; set; }
    public required string Example { get; init; }
    
    public int PhraseId { get; set; }
    public Phrase Phrase { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not PhraseExample other)
        {
            return false;
        }
        
        return Example == other.Example;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Example);
    }
}