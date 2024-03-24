using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhraseFinder.WebApp.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    [BindProperty]
    [MaxLength(10_000, ErrorMessage = "El texto es demasiado largo (máximo 10.000 caracteres)")]
    public string Text { get; set; } = "";
    
    [BindProperty]
    [MaxLength(10_000, ErrorMessage = "El fichero de texto es demasiado grande (máximo 10MB)")]
    public IFormFile? TextFile { get; set; }

    [BindProperty]
    public bool CanSubmit => !string.IsNullOrWhiteSpace(Text) || TextFile != null;

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!CanSubmit)
        {
            ModelState.AddModelError(string.Empty, "Introduzca un texto o seleccione un fichero de texto plano.");
            return Page();
        }

        if (TextFile != null)
        {
            using var reader = new StreamReader(TextFile.OpenReadStream());
            Text = await reader.ReadToEndAsync();
        }

        Console.WriteLine($"Text: {Text}");
        Console.WriteLine(TextFile?.Name ?? "No file selected");
        return RedirectToPage("/Phrases", new { text = Text });
    }
}