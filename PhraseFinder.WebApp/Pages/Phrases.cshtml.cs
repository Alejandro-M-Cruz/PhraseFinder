using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Pages;

public class PhrasesModel(IPhraseFinderService phraseFinder) : PageModel
{
	public string Text { get; set; } = "";

	public FoundPhrase[]? FoundPhrases { get; set; }

	public async Task<IActionResult> OnGetAsync()
	{
		Text = TempData.Peek("Text") as string ?? "";

		if (string.IsNullOrWhiteSpace(Text))
		{
			TempData.Clear();
			return RedirectToPage("/Index");
		}
		
		try
		{
			FoundPhrases = (await phraseFinder.FindPhrasesAsync(Text))
				.OrderBy(p => p.StartIndex)
				.ToArray();
		}
		catch
		{
			ModelState.AddModelError(
				string.Empty, 
				"Se ha producido un error. Por favor, inténtelo de nuevo más tarde.");
		}

		return Page();
	}
}