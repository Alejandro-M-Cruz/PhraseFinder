using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public class PhraseDefinition
{
    public int PhraseDefinitionId { get; set; }
    
    [Display(Name = "Definición")]
    [StringLength(maximumLength: 1000, MinimumLength = 1,
        ErrorMessage = "La definición debe tener entre 1 y 1000 caracteres")]
    public required string Definition { get; set; }
    
    public int PhraseId { get; set; }
}