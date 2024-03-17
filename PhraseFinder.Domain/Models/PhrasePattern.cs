using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Patrones de expresión o locución")]
public class PhrasePattern
{
    [Column("ID")]
    public int PhrasePatternId { get; set; }

    [Column("Nombre")]
    [MaxLength(255)]
    public string Name { get; set; }

    [Column("Patrón")]
    public string Pattern { get; set; }

    [Column("ID de expresión o locución")]
    public int PhraseId { get; set; }

    public Phrase Phrase { get; set; }
}