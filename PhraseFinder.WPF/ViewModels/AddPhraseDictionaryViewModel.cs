using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class AddPhraseDictionaryViewModel(
    IPhraseDictionaryService phraseDictionaryService,
    INavigationService navigationService) : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPhraseDictionaryCommand))]
    private string? _phraseDictionaryName = string.Empty;

    [ObservableProperty] 
    private string? _phraseDictionaryDescription;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPhraseDictionaryCommand))]
    private PhraseDictionaryFormat? _selectedPhraseDictionaryFormat;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPhraseDictionaryCommand))]
    private string? _phraseDictionaryFilePath;

    public bool CanAddPhraseDictionary =>
        !string.IsNullOrWhiteSpace(PhraseDictionaryName) &&
        !string.IsNullOrWhiteSpace(PhraseDictionaryFilePath) &&
        SelectedPhraseDictionaryFormat != null;

    [ObservableProperty]
    private bool _isDictionaryBeingAdded;

    [RelayCommand(CanExecute = nameof(CanAddPhraseDictionary))]
    public async Task AddPhraseDictionary()
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = PhraseDictionaryName!,
            Format = (PhraseDictionaryFormat)SelectedPhraseDictionaryFormat!,
            Description = PhraseDictionaryDescription,
            FilePath = PhraseDictionaryFilePath!
        };
        IsDictionaryBeingAdded = true;
        await phraseDictionaryService.AddPhraseDictionaryAsync(phraseDictionary);
        IsDictionaryBeingAdded = false;
        navigationService.NavigateTo<PhraseDictionariesViewModel>();
    }

    [RelayCommand]
    public void PickPhraseDictionaryFile()
    {
        OpenFileDialog openFileDialog = new()
        {
            Multiselect = false
        };
        if (openFileDialog.ShowDialog() == true)
        {
            PhraseDictionaryFilePath = openFileDialog.FileName;
        }
    }

    [RelayCommand]
    public void NavigateToPhraseDictionaries()
    {
        navigationService.NavigateTo<PhraseDictionariesViewModel>();
    }
}
