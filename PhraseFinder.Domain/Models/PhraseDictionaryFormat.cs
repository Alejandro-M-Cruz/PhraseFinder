using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public enum PhraseDictionaryFormat
{
    [Display(Name = "DLE (Diccionario de la lengua española) en texto plano")] 
    DleTxt
}
