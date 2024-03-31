using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Locuciones_y_expresiones")]
public record Phrase
{
    [Column("ID_Locucion")]
    public int PhraseId { get; set; }

    [Column("Locucion")]
    [MaxLength(255)]
    public required string Value { get; set; }

    [Column("Palabra_base")]
    [MaxLength(255)]
    public required string BaseWord { get; set; }

    [Column("Variante")]
    [MaxLength(255)]
    public string Variant { get; set; }

    [Column("Patron")]
    [MaxLength(255)]
    public string Pattern { get; set; }

    [Column("Categorias")]
    [MaxLength(255)]
    public string Categories { get; set; }

    [Column("Revisado")] 
    public bool Reviewed { get; set; } = false;

    [Column("ID_Diccionario")]
    public int PhraseDictionaryId { get; set; }

    public PhraseDictionary PhraseDictionary { get; set; }
    
    public ICollection<PhraseDefinition> Definitions { get; set; } = [];
    
    public override string ToString()
    {
        return $"{Value}; {Variant}; {Pattern}";
    }
}
