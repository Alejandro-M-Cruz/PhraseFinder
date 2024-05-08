using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Pages;

public class PhrasesModel(IPhraseFinderService phraseFinder) : PageModel
{
	[BindProperty] 
	public string Text { get; set; } = "";

    [BindProperty] 
    public FoundPhrase[] FoundPhrases { get; set; } = [];

    [BindProperty] 
    public string? Error { get; set; }

    public async Task OnGetAsync(string filePath)
    {
	    try
	    {
		    using (var reader = new StreamReader(filePath))
		    {
				Text = await reader.ReadToEndAsync();
		    }
		    FoundPhrases = (await phraseFinder.FindPhrasesAsync(Text))
			    .OrderBy(p => p.StartIndex)
			    .ToArray();
		}
	    catch
	    {
		    Error = "Se ha producido un error. Por favor, inténtelo de nuevo más tarde";
		    System.IO.File.Delete(filePath);
		}
	}
}