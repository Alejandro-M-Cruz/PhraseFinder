using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PhraseFinder.WebApp.Options;

namespace PhraseFinder.WebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Por favor, introduzca un texto.")]
    public string Text { get; set; } = string.Empty;

    private readonly TextValidationOptions _options;

    public IndexModel(IOptions<TextValidationOptions> textValidationOptions)
    {
        _options = textValidationOptions.Value;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Text.Length < _options.MinLength)
        {
            var characters = _options.MinLength == 1 ? "car�cter" : "caracteres";
            ModelState.AddModelError(
                nameof(Text), 
                $"El texto es demasiado corto (m�nimo {_options.MinLength} {characters}).");
            return Page();
        }

        if (Text.Length > _options.MaxLength)
        {
            var characters = _options.MaxLength == 1 ? "car�cter" : "caracteres";
            ModelState.AddModelError(
                nameof(Text),
                $"El texto es demasiado largo (m�ximo {_options.MaxLength} {characters}).");
            return Page();
        }

        TempData["Text"] = Text;
        return RedirectToPage("/Phrases");
    }
}