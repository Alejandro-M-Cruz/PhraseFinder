using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Diccionarios")]
public class PhraseDictionary
{
    [Column("ID_Diccionario")]
    public int PhraseDictionaryId { get; set; }

    [Column("Nombre")]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Column("Descripcion")]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column("Formato")]
    public required PhraseDictionaryFormat Format { get; set; }

    [Column("Ruta_fichero")]
    [MaxLength(32_767)]
    public required string FilePath { get; set; }

    [Column("Fecha_creacion")]
    public DateTime AddedAt { get; set; }
    
    public ICollection<Phrase> Phrases { get; set; } = [];
}