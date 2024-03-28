using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Definiciones")]
public class PhraseDefinition
{
    [Column("ID")]
    public int PhraseDefinitionId { get; set; }

    [Column("Definición")]
    [MaxLength(1000)]
    public required string Definition { get; set; }

    [Column("ID de expresión o locución")]
    public int PhraseId { get; set; }

    public ICollection<PhraseExample> Examples { get; set; } = [];
}