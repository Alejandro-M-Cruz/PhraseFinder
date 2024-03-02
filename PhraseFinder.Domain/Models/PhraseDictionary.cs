using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public class PhraseDictionary
{
    [Display(Name = "ID")]
    public int PhraseDictionaryId { get; set; }
    
    [Display(Name = "Nombre")]
    [StringLength(maximumLength: 255, MinimumLength = 1, ErrorMessage="El nombre debe tener entre 1 y 255 caracteres")]
    public required string Name { get; set; }
    
    [Display(Name = "Descripción")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "La descripción debe tener entre 1 y 1000 caracteres")]
    public string? Description { get; set; }
    
    [Display(Name = "Formato")]
    public required PhraseDictionaryFormat Format { get; set; }
    
    [Display(Name = "Ruta del fichero")]
    [StringLength(maximumLength: 500, MinimumLength = 1,
        ErrorMessage = "La ruta del fichero debe tener entre 1 y 500 caracteres")]
    public required string FilePath { get; set; }
    
    [Display(Name = "Fecha de creación")]
    public DateTime AddedAt { get; set; }
    
    public ICollection<Phrase> Phrases { get; set; } = [];
}