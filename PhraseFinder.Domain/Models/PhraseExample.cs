using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Ejemplos")]
public class PhraseExample
{
    [Column("ID")]
    public int PhraseExampleId { get; set; }

    [Column("Ejemplo")]
    [Display(Name = "Ejemplo")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "El ejemplo debe tener entre 1 y 1000 caracteres")]
    public required string Example { get; set; }

    [Column("ID de definición")]
    public int PhraseDefinitionId { get; set; }
}