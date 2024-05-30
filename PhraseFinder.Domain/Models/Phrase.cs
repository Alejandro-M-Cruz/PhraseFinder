using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Locuciones_y_expresiones")]
public record Phrase
{
    [Column("ID_Locucion")]
    public int PhraseId { get; set; }

    [Column("Locucion_o_expresion")]
    public required string Value { get; init; }

    [Column("Palabra_base")]
    [MaxLength(255)]
    public required string BaseWord { get; init; }

    [Column("Categorias")]
    [MaxLength(255)]
    public required string Categories { get; init; }

    [Column("Revisado")] 
    public bool Reviewed { get; set; } = false;

    [Column("ID_Diccionario")]
    public int PhraseDictionaryId { get; set; }

    public ICollection<PhraseDefinition> Definitions { get; set; } = [];

    public ICollection<PhrasePattern> Patterns { get; set; } = [];
    
    public override string ToString()
    {
        return $"{Value} ({BaseWord})";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, BaseWord, Categories);
    }

    public virtual bool Equals(Phrase? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value == other.Value && 
               BaseWord == other.BaseWord && 
               Categories == other.Categories;
    }
}
