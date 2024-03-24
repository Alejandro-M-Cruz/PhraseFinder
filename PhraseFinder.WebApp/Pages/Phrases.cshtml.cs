using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhraseFinderServiceReference;

namespace PhraseFinder.WebApp.Pages;

public class PhrasesModel(IPhraseFinderService phraseFinder) : PageModel
{
    [BindProperty]
    public string Text => "Alguno que otro traía un libro. Esto es un texto de ejemplo.\nBlablabla.\nAlgunos que otros algo algo.";

    [BindProperty] 
    public IEnumerable<FoundPhrase> FoundPhrases { get; set; } = [];

    [BindProperty] 
    public bool IsLoading { get; set; }

    public async Task OnGetAsync()
    {
        FoundPhrases = await phraseFinder.FindPhrasesAsync(Text);
    }
}