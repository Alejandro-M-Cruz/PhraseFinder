using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Pages;

public class PhrasesModel(IPhraseFinderService phraseFinder) : PageModel
{
	public string Text { get; private set; } = "";

	public FoundPhrase[]? FoundPhrases { get; private set; }

	public bool PhrasesAreLoaded { get; private set; }

    public IActionResult OnGet()
    {
        if (!RetrieveText())
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

	public async Task<IActionResult> OnPostAsync()
    {
        if (!RetrieveText())
        {
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
				"Se ha producido un error. Por favor, int�ntelo de nuevo m�s tarde.");
		}

		PhrasesAreLoaded = true;
		return Page();
	}

    private bool RetrieveText()
    {
        Text = TempData.Peek("Text") as string ?? "";

        if (string.IsNullOrWhiteSpace(Text))
        {
            TempData.Clear();
            return false;
        }

        return true;
    }
}