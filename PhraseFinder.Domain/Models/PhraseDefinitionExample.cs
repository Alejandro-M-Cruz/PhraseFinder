using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Ejemplos")]
public class PhraseDefinitionExample
{
    [Column("ID")]
    public int PhraseDefinitionExampleId { get; set; }

    [Column("Ejemplo")]
    [Display(Name = "Ejemplo")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "El ejemplo debe tener entre 1 y 1000 caracteres")]
    public required string Example { get; set; }

    [Column("ID de definici�n")]
    public int PhraseDefinitionId { get; set; }
}