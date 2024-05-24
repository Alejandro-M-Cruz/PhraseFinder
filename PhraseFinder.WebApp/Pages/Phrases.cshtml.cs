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
            var phraseAnalysis = await phraseFinder.FindPhrasesAsync(Text);
			FoundPhrases = phraseAnalysis.FoundPhrases
				.OrderBy(p => p.StartIndex)
				.ToArray();
            Text = phraseAnalysis.ProcessedText;
            TempData["Text"] = Text;
        }
		catch
		{
			ModelState.AddModelError(
				string.Empty,
				"Se ha producido un error. Por favor, inténtelo de nuevo más tarde.");
		}

		PhrasesAreLoaded = true;
		return Page();
	}

    private bool RetrieveText()
    {
        Text = TempData.Peek("Text") as string ?? "";

        if (!string.IsNullOrWhiteSpace(Text))
        {
            return true;
        }

        TempData.Clear();
        return false;
    }
}