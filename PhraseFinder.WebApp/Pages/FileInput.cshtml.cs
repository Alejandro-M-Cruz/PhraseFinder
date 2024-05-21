using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PhraseFinder.WebApp.Options;

namespace PhraseFinder.WebApp.Pages;

public class FileInputModel : PageModel
{
    [BindProperty]
	[Required(ErrorMessage = "Por favor, seleccione un fichero de texto.")]
	public IFormFile? TextFile { get; set; }
	
    private readonly TextFileValidationOptions _options;
	
    public FileInputModel(IOptions<TextFileValidationOptions> textFileValidationOptions)
    {
		_options = textFileValidationOptions.Value;
    }

    public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid || TextFile == null)
		{
			return Page();
		}

		if (TextFile.Length > _options.MaxSizeKiloBytes * 1024)
		{
			ModelState.AddModelError(
			nameof(TextFile), 
				$"El fichero es demasiado grande (m�ximo {_options.MaxSizeKiloBytes}KB).");
			return Page();
		}

		try
		{
			using var reader = new StreamReader(TextFile.OpenReadStream());
			var text = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(text) || text.Length < 3)
            {
                var characters = _options.MinContentLength == 1 ? "car�cter" : "caracteres";
                ModelState.AddModelError(
                    nameof(TextFile),
                    $"El fichero est� vac�o o es demasiado corto (m�nimo {_options.MinContentLength} {characters}.");
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