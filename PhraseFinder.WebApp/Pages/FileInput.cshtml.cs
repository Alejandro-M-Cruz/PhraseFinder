using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhraseFinder.WebApp.Pages;

public class FileInputModel(ILogger<IndexModel> logger) : PageModel
{
	[BindProperty]
	public IFormFile? TextFile { get; set; }


	public void OnGet()
	{
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		if (TextFile == null)
		{
			ModelState.AddModelError(
				nameof(TextFile), 
				"Por favor, seleccione un fichero");
			return Page();
		}

		logger.LogInformation($"File length: {TextFile!.Length}");

		if (TextFile!.Length > 10 * 1024)
		{
			ModelState.AddModelError(
			nameof(TextFile), 
				$"El fichero es demasiado grande (m�ximo 10KB)");
			return Page();
		}

		var tempFilePath = Path.GetTempFileName();
		await using (var stream = new FileStream(tempFilePath, FileMode.Create))
		{
			await TextFile!.CopyToAsync(stream);
		}

		System.IO.File.Delete(tempFilePath);

		return RedirectToPage("/Phrases", new { filePath = tempFilePath });
	}
}