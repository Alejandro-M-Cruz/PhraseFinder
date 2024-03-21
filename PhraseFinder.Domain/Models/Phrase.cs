using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFinder.Domain.Models;

[Table("Expresiones y locuciones")]
public class Phrase
{
    [Column("ID")]
    public int PhraseId { get; set; }

    [Column("Expresión o locución")]
    [MaxLength(255)]
    public required string Value { get; set; }

    [Column("Palabra base")]
    [StringLength(maximumLength: 255, MinimumLength = 1,
        ErrorMessage = "La palabra base debe tener entre 1 y 255 caracteres")]
    public required string BaseWord { get; set; }

    [Column("ID de diccionario")]
    public int PhraseDictionaryId { get; set; }

    public PhraseDictionary PhraseDictionary { get; set; }
    
    public ICollection<PhraseDefinition> Definitions { get; set; } = [];
    
    public override string ToString()
    {
        return Value;
    }
}
