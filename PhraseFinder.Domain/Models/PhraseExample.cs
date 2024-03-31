using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Ejemplos")]
public class PhraseExample
{
    [Column("ID_Ejemplo")]
    public int PhraseExampleId { get; set; }

    [Column("Ejemplo")]
    [MaxLength(1000)]
    public required string Example { get; set; }

    [Column("ID_Definicion")]
    public int PhraseDefinitionId { get; set; }
}
