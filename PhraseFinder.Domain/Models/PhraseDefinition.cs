namespace PhraseFinder.Domain.Models;

public class PhraseDefinition
{
    public int PhraseDefinitionId { get; set; }
    public required string Definition { get; init; }
    
    public int PhraseId { get; set; }
    public Phrase Phrase { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not PhraseDefinition other)
        {
            return false;
        }
        
        return Definition == other.Definition;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Definition);
    }
}