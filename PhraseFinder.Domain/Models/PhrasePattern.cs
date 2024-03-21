using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Patrones de expresión o locución")]
public class PhrasePattern
{
    [Column("ID")]
    public int PhrasePatternId { get; set; }

    [Column("Expresión o locución de origen")]
    [MaxLength(255)]
    public required string Source { get; set; }

    [Column("Patrón")]
    public required string Pattern { get; set; }

    [Column("Verificado")] 
    public required bool IsVerified { get; set; } = true;

    [Column("ID de expresión o locución")]
    public int PhraseId { get; set; }

    public Phrase Phrase { get; set; }
}