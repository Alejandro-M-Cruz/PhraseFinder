namespace PhraseFinder.Domain.Models;

public class PhraseEntry
{
    public required string Name { get; set; }
    public required string BaseWord { get; set; }
    public IDictionary<string, ICollection<string>> DefinitionToExamples { get; } = 
        new Dictionary<string, ICollection<string>>();

    public Phrase ToPhrase()
    {
        return new Phrase
        {
            Name = Name,
            RegExPattern = Name,
            BaseWord = BaseWord,
            Definitions = DefinitionToExamples.Select(d =>
            {
                return new PhraseDefinition
                {
                    Definition = d.Key, 
                    Examples = d.Value
                        .Select(example => new PhraseExample
                        {
                            Example = example
                        })
                        .ToList()
                };
            }).ToList(),
        };
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not PhraseEntry other)
        {
            return false;
        }

        return Name == other.Name && BaseWord == other.BaseWord && 
               DefinitionToExamplesEquals(other.DefinitionToExamples);
    }

    private bool DefinitionToExamplesEquals(IDictionary<string, ICollection<string>> other)
    {
        return DefinitionToExamples.All(d => other[d.Key].SequenceEqual(d.Value));
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, BaseWord, DefinitionToExamples);
    }
}