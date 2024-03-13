using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Diccionarios")]
public class PhraseDictionary
{
    [Column("ID")]
    [Display(Name = "ID")]
    public int PhraseDictionaryId { get; set; }

    [Column("Nombre")]
    [Display(Name = "Nombre")]
    [StringLength(maximumLength: 255, MinimumLength = 1, ErrorMessage="El nombre debe tener entre 1 y 255 caracteres")]
    public required string Name { get; set; }

    [Column("Descripción")]
    [Display(Name = "Descripción")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "La descripción debe tener entre 1 y 1000 caracteres")]
    public string? Description { get; set; }

    [Column("Formato")]
    [Display(Name = "Formato")]
    public required PhraseDictionaryFormat Format { get; set; }

    [Column("Ruta del fichero")]
    [Display(Name = "Ruta del fichero")]
    [StringLength(maximumLength: 500, MinimumLength = 1,
        ErrorMessage = "La ruta del fichero debe tener entre 1 y 500 caracteres")]
    public required string FilePath { get; set; }

    [Column("Fecha de creación")]
    [Display(Name = "Fecha de creación")]
    public DateTime AddedAt { get; set; }
    
    public ICollection<Phrase> Phrases { get; set; } = [];
}