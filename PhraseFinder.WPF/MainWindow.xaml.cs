using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PhraseFinder.Data;
using PhraseFinder.Domain.Models;
using PhraseFinder.Domain.Services;

namespace PhraseFinder.WPF;

public partial class MainWindow : Window
{
    private PhraseFinderDbContext _dbContext;

    public MainWindow(PhraseFinderDbContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        var phraseDictionaries = await _dbContext.PhraseDictionaries.ToListAsync();
        PhraseDictionaryDataGrid.ItemsSource = phraseDictionaries;
    }

    private void PhraseDictionaryDataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        UpdatePhraseDictionaryActions(areEnabled: PhraseDictionaryDataGrid.SelectedItem != null);
    }

    private void UpdatePhraseDictionaryActions(bool areEnabled = true)
    {
        PhraseListButton.IsEnabled = areEnabled;
        PhraseDictionaryDeleteButton.IsEnabled = areEnabled;
    }

    private async void PhraseDictionaryAddButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Multiselect = false
        };
        bool? fileWasPicked = dialog.ShowDialog();
        if (fileWasPicked != true)
        {
            return;
        }
        var reader = new DleTxtPhraseDictionaryReader(dialog.FileName);
        var phraseDictionary = new PhraseDictionary
        {
            Name = "New Dictionary",
            Format = PhraseDictionaryFormat.DleTxt,
            FilePath = dialog.FileName
        };
        _dbContext.PhraseDictionaries.Add(phraseDictionary);
        await _dbContext.SaveChangesAsync();
    }

    private async void PhraseDictionaryDeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var phraseDictionary = (PhraseDictionary)PhraseDictionaryDataGrid.SelectedItem;
        _dbContext.PhraseDictionaries.Remove(phraseDictionary);
        await _dbContext.SaveChangesAsync();
    }
}