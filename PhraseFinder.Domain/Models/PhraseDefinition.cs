using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Definiciones")]
public class PhraseDefinition
{
    [Column("ID_Definicion")]
    public int PhraseDefinitionId { get; set; }

    [Column("Definicion")]
    [MaxLength(1000)]
    public required string Definition { get; set; }

    [Column("ID_Locucion")]
    public int PhraseId { get; set; }

    public ICollection<PhraseExample> Examples { get; set; } = [];
}