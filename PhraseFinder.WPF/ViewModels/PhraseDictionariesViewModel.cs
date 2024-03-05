using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class PhraseDictionariesViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(DeletePhraseDictionaryCommand), 
        nameof(NavigateToPhrasesCommand))]
    private PhraseDictionary? _selectedPhraseDictionary;

    public bool IsPhraseDictionarySelected() => SelectedPhraseDictionary != null;

    public ObservableCollection<PhraseDictionary> PhraseDictionaries { get; } = [];

    private readonly IPhraseDictionaryService _phraseDictionaryService;

    private readonly INavigationService _navigationService;

    public PhraseDictionariesViewModel(
        IPhraseDictionaryService phraseDictionaryService,
        INavigationService navigationService)
    {
        _phraseDictionaryService = phraseDictionaryService;
        _navigationService = navigationService;
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
        _navigationService.NavigateTo<AddPhraseDictionaryViewModel>();
    }
}