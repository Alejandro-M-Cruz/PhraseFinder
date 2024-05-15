using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhraseFinder.WebApp.Pages;

public class FileInputModel : PageModel
{
	[BindProperty]
	[Required(ErrorMessage = "Por favor, seleccione un fichero.")]
	public IFormFile? TextFile { get; set; }

	public void OnGet()
	{
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid || TextFile == null)
		{
			return Page();
		}

		if (TextFile.Length > 10 * 1024)
		{
			ModelState.AddModelError(
			nameof(TextFile), 
				"El fichero es demasiado grande (m�ximo 10KB).");
			return Page();
		}

		try
		{
			using var reader = new StreamReader(TextFile.OpenReadStream());
			var text = await reader.ReadToEndAsync();
			if (string.IsNullOrWhiteSpace(text) || text.Length < 3)
			{
				ModelState.AddModelError(
					nameof(TextFile), 
					"El fichero est� vac�o o es demasiado corto (m�nimo 3 caracteres).");
				return Page();
			}
			TempData["Text"] = text;
		}
		catch
		{
			ModelState.AddModelError(
				nameof(TextFile), 
				"Se ha producido un error. Por favor, int�ntelo de nuevo m�s tarde.");
			return Page();
		}

		return RedirectToPage("/Phrases");
	}

}