using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhraseFinder.WebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Por favor, introduzca un texto")]
    [MaxLength(10_000, ErrorMessage = "El texto es demasiado largo (máximo 10.000 caracteres)")]
    [MinLength(3, ErrorMessage= "El texto es demasiado corto (mínimo 3 caracteres)")]
    public string Text { get; set; } = "";

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
	    if (!ModelState.IsValid)
	    {
			return Page();
		}

	    if (string.IsNullOrWhiteSpace(Text))
	    {
            ModelState.AddModelError(nameof(Text), "Por favor, introduzca un texto.");
            return Page();
	    }

	    if (Text.Length > 10_000)
	    {
			ModelState.AddModelError(nameof(Text), "El texto es demasiado largo (máximo 10.000 caracteres).");
			return Page();
		}

		var tempFilePath = Path.GetTempFileName();
		try
		{
			await System.IO.File.WriteAllTextAsync(tempFilePath, Text);
		}
		catch
		{
			System.IO.File.Delete(tempFilePath);
			ModelState.AddModelError(
				nameof(Text),
				"Se ha producido un error. Por favor, inténtelo de nuevo más tarde.");
			return Page();
		}

		return RedirectToPage("/Phrases", new { filePath = tempFilePath });
    }
}