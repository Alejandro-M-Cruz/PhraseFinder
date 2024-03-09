using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Messages;
using System.Collections.ObjectModel;
using HandyControl.Tools.Extension;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class PhrasesViewModel : ObservableObject
{
    public PhraseDictionary PhraseDictionary { get; }
    private readonly IPhraseService _phraseService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<Phrase> Phrases { get; } = [];
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DisplayPhraseDetailsCommand))]
    private Phrase? _selectedPhrase;

    private bool IsPhraseSelected => SelectedPhrase != null;
    
    [ObservableProperty]
    private Func<Phrase, object> _orderBy = p => p.PhraseId;

    [ObservableProperty] 
    private int _skip = 0;
    
    [ObservableProperty] 
    private int _take = 100;

    public PhrasesViewModel(IPhraseService phraseService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        PhraseDictionary = 
            WeakReferenceMessenger.Default.Send<PhraseDictionaryRequestMessage>();
        Console.WriteLine(PhraseDictionary.Name);
        _phraseService = phraseService;
        LoadPhraseDictionaryPhrasesCommand.Execute(null);
    }

    [RelayCommand]
    public void LoadPhraseDictionaryPhrases()
    {
        var phrases = _phraseService.GetPhrases(PhraseDictionary, OrderBy, Skip, Take);
        Phrases.AddRange(phrases);
    }

    [RelayCommand]
    public void NavigateToPhraseDictionaries()
    {
        _navigationService.NavigateTo<PhraseDictionariesViewModel>();
    }

    [RelayCommand(CanExecute = nameof(IsPhraseSelected))]
    public void DisplayPhraseDetails()
    {

    }
}