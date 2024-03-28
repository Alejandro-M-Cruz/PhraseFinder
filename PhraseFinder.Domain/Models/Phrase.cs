using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Expresiones y locuciones")]
public record Phrase
{
    [Column("ID")]
    public int PhraseId { get; set; }

    [Column("Expresión o locución")]
    [MaxLength(255)]
    public required string Value { get; set; }

    [Column("Palabra base")]
    [MaxLength(255)]
    public required string BaseWord { get; set; }

    [Column("Variante")]
    [MaxLength(255)]
    public string Variant { get; set; }

    [Column("Patrón")]
    [MaxLength(255)]
    public string Pattern { get; set; }

    [Column("Revisado")] 
    public bool Reviewed { get; set; } = false;

    [Column("ID de diccionario")]
    public int PhraseDictionaryId { get; set; }

    public PhraseDictionary PhraseDictionary { get; set; }
    
    public ICollection<PhraseDefinition> Definitions { get; set; } = [];
    
    public override string ToString()
    {
        return $"{Value}; {Variant}; {Pattern}";
    }
}
