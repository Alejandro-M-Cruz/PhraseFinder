namespace PhraseFinder.Domain.Models;

public class PhraseEntry
{
    public required string Name { get; init; }
    public required string BaseWord { get; init; }
    public ICollection<string> Definitions { get; } = [];
    public ICollection<string> Examples { get; } = [];

    public Phrase ToPhrase()
    {
        return new Phrase
        {
            Name = Name,
            RegExPattern = Name,
            BaseWord = BaseWord,
            Definitions = Definitions.Select(d => new PhraseDefinition { Definition = d }).ToList(),
            Examples = Examples.Select(d => new PhraseExample { Example = d }).ToList()
        };
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not PhraseEntry other)
        {
            return false;
        }

        return Name == other.Name && BaseWord == other.BaseWord &&
               Definitions.SequenceEqual(other.Definitions) && 
               Examples.SequenceEqual(other.Examples);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, BaseWord, Definitions, Examples);
    }
}