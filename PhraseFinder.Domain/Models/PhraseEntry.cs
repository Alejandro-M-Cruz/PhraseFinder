namespace PhraseFinder.Domain.Models;

public class PhraseEntry
{
    public required string Name { get; set; }
    public required string BaseWord { get; set; }
    public ISet<string> Categories { get; set; } = new HashSet<string>();
    public IDictionary<string, ICollection<string>> DefinitionToExamples { get; } = 
        new Dictionary<string, ICollection<string>>();

    public Phrase ToPhrase()
    {
        return new Phrase
        {
            Value = Name,
            BaseWord = BaseWord,
            Categories = string.Join(", ", Categories),
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
               Categories.SequenceEqual(other.Categories) &&
               DefinitionToExamplesEquals(other.DefinitionToExamples);
    }

    private bool DefinitionToExamplesEquals(IDictionary<string, ICollection<string>> other)
    {
        return DefinitionToExamples
	        .All(d => other[d.Key].SequenceEqual(d.Value));
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, BaseWord, DefinitionToExamples);
    }
}