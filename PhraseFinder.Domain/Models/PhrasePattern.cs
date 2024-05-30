using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Patrones")]
public record PhrasePattern
{
    [Column("ID_Patron")]
    public int PhrasePatternId { get; set; }

    [Column("Locucion_o_expresion")]
    [MaxLength(255)]
    public required string Phrase { get; set; }

    [Column("Variante")]
    [MaxLength(255)]
    public required string Variant { get; set; }

    [Column("Patron")]
    [MaxLength(255)]
    public required string Pattern { get; set; }

    [Column("Palabra_base")]
    [MaxLength(255)]
    public required string BaseWord { get; set; }

    [Column("ID_Locucion")]
    public int PhraseId { get; set; }
}
