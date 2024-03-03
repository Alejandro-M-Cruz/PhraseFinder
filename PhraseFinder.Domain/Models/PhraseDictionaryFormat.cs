using System.ComponentModel.DataAnnotations;

namespace PhraseFinder.Domain.Models;

public enum PhraseDictionaryFormat
{ 
    [Display(Name = "DLE en texto plano")] 
    DleTxt
}
