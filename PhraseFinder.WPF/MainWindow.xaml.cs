using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhraseFinder.Data;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.WPF;

public partial class MainWindow : Window
{
    private PhraseFinderDbContext _dbContext;

    public MainWindow(PhraseFinderDbContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        int phraseDictionaryCount = _dbContext.PhraseDictionaries.Count();
        if (phraseDictionaryCount > 0)
        {
            ExampleTextBox.Text = _dbContext.PhraseDictionaries.First().Name;
        }
        else
        {
            ExampleTextBox.Text = "No hay diccionarios" ;
        }
    }

    private async void ExampleButton_OnClick(object sender, RoutedEventArgs e)
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = "Ejemplo",
            Description = "Un ejemplo de diccionario",
            Path = "ejemplo.txt",
            Format = PhraseDictionaryFormat.DleTxt
        };
        _dbContext.PhraseDictionaries.Add(phraseDictionary);
        await _dbContext.SaveChangesAsync();
    }
}