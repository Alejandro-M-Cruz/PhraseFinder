using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Definiciones")]
public class PhraseDefinition
{
    [Column("ID")]
    public int PhraseDefinitionId { get; set; }

    [Column("Definición")]
    [Display(Name = "Definición")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "La definición debe tener entre 1 y 1000 caracteres")]
    public required string Definition { get; set; }

    [Column("ID de expresión o locución")]
    public int PhraseId { get; set; }

    public ICollection<PhraseDefinitionExample> Examples { get; set; } = [];
}