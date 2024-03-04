using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.WPF.ViewModels;

public partial class PhraseDictionariesViewModel : ObservableObject
{
    private readonly IPhraseDictionaryService _phraseDictionaryService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(DeletePhraseDictionaryCommand), 
        nameof(NavigateToPhrasesCommand))]
    private PhraseDictionary? _selectedPhraseDictionary;

    public bool IsPhraseDictionarySelected() => SelectedPhraseDictionary != null;

    public ObservableCollection<PhraseDictionary> PhraseDictionaries { get; } = [];

    public PhraseDictionariesViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        _phraseDictionaryService = phraseDictionaryService;
        LoadPhraseDictionariesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadPhraseDictionaries()
    {
        var phraseDictionaries = 
            await _phraseDictionaryService.GetPhraseDictionariesAsync();
        foreach (var phraseDictionary in phraseDictionaries)
        {
            PhraseDictionaries.Add(phraseDictionary);
        }
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public async Task DeletePhraseDictionary()
    {
        if (SelectedPhraseDictionary != null)
        {
            await _phraseDictionaryService.DeletePhraseDictionaryAsync(
                               SelectedPhraseDictionary);
            PhraseDictionaries.Remove(SelectedPhraseDictionary);
        }
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public void NavigateToPhrases()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    public void NavigateToAddPhraseDictionary()
    {
        throw new NotImplementedException();
    }
}