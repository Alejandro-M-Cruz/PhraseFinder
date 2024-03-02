using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public class PhraseExample
{
    public int PhraseExampleId { get; set; }
    
    [Display(Name = "Ejemplo")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "El ejemplo debe tener entre 1 y 1000 caracteres")]
    public required string Example { get; set; }
    
    public int PhraseId { get; set; }
}