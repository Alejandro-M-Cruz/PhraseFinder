using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public class Phrase
{
    public int PhraseId { get; set; }
    
    [StringLength(maximumLength: 255, MinimumLength = 1,
        ErrorMessage = "El nombre debe tener entre 1 y 255 caracteres")]
    public required string Name { get; set; }
    
    [Display(Name = "Expresión regular")]
    [StringLength(maximumLength: 2000, MinimumLength = 1,
        ErrorMessage = "El patrón de expresión regular debe tener entre 1 y 2000 caracteres")]
    public required string RegExPattern { get; set; }
    
    [StringLength(maximumLength: 255, MinimumLength = 1,
        ErrorMessage = "La palabra base debe tener entre 1 y 255 caracteres")]
    public required string BaseWord { get; set; }
    
    public int PhraseDictionaryId { get; set; }

    public PhraseDictionary PhraseDictionary { get; set; }
    
    public ICollection<PhraseDefinition> Definitions { get; set; } = [];
    
    public override string ToString()
    {
        return Name;
    }
}
