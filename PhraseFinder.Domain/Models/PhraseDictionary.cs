namespace PhraseFinder.Domain.Models;

public class PhraseDictionary
{
    public int PhraseDictionaryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required PhraseDictionaryFormat  Format { get; set; }
    public required string Path { get; set; }
    public DateTime AddedAt { get; set; }

    public ICollection<Phrase> Phrases { get; set; } = [];
}