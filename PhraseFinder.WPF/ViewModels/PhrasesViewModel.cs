using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Tools.Extension;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Messages;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class PhrasesViewModel : ObservableObject
{
    public PhraseDictionary PhraseDictionary { get; }
    private readonly IPhraseService _phraseService;
    private readonly INavigationService _navigationService;

    public ObservableCollection<Phrase> DisplayedPhrases { get; } = [];
    
    [ObservableProperty] 
    private PhraseQueryOptions _options = new();

    [ObservableProperty]
    private int _currentPage;

	[ObservableProperty]
	private int _currentTotalPages;

    public PhrasesViewModel(IPhraseService phraseService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        PhraseDictionary = 
            WeakReferenceMessenger.Default.Send<PhraseDictionaryRequestMessage>();
        _phraseService = phraseService;
        LoadPhrases();
    }

    [RelayCommand]
    public void LoadPhrases()
    {
	    Options.Page = CurrentPage;
        Options.TotalPages = CurrentTotalPages;
        DisplayedPhrases.Clear();
        var phrases = _phraseService.GetPhrases(PhraseDictionary, Options);
	    DisplayedPhrases.AddRange(phrases);
        CurrentPage = Options.Page;
        CurrentTotalPages = Options.TotalPages;
    }

    [RelayCommand]
    public void NavigateToPhraseDictionaries()
    {
        _navigationService.NavigateTo<PhraseDictionariesViewModel>();
    }
}