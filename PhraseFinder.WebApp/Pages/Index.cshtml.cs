using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhraseFinder.WebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Por favor, introduzca un texto.")]
    [MinLength(3, ErrorMessage = "El texto es demasiado corto (mínimo 3 caracteres).")]
    [MaxLength(10_000, ErrorMessage = "El texto es demasiado largo (máximo 10.000 caracteres).")]
    public string Text { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        TempData["Text"] = Text;
        return RedirectToPage("/Phrases");
    }
}