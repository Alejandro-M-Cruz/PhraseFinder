namespace PhraseFinder.Domain.Models;

public class Phrase
{
    public int PhraseId { get; set; }
    public required string Name { get; init; }
    public string? RegExPattern { get; set; }
    public required string BaseWord { get; init; }
    
    public int PhraseDictionaryId { get; set; } 
    public PhraseDictionary PhraseDictionary { get; set; }

    public ICollection<PhraseDefinition> Definitions { get; set; } = [];
    public ICollection<PhraseExample> Examples { get; set; } = [];
    
    public override string ToString()
    {
        return $"Phrase(Id: {PhraseId}, Name: {Name}, Pattern: {RegExPattern}, BaseWord: {BaseWord}, " +
               $"DictionaryId: {PhraseDictionaryId})";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Phrase other)
        {
            return false;
        }
        
        return Name == other.Name && BaseWord == other.BaseWord &&
               Definitions.SequenceEqual(other.Definitions) && Examples.SequenceEqual(other.Examples);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, BaseWord);
    }
}
