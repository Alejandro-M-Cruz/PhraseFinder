using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Tools.Extension;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Extensions;
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
    [NotifyCanExecuteChangedFor(nameof(DisplayPhraseDetailsCommand))]
    private Phrase? _selectedPhrase;

    private bool IsPhraseSelected => SelectedPhrase != null;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchPhrasesCommand))]
    private string? _searchText;

    [ObservableProperty] 
    private int _currentPage = 1;

    [ObservableProperty] 
    private int _elementsPerPage = ElementsPerPageOptions[DefaultElementsPerPageIndex];

    public static List<int> ElementsPerPageOptions => [10, 20, 50, 100];
    public static int DefaultElementsPerPageIndex => 1;

    [ObservableProperty] 
    private int _totalPages;

    public PhrasesViewModel(IPhraseService phraseService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        PhraseDictionary = 
            WeakReferenceMessenger.Default.Send<PhraseDictionaryRequestMessage>();
        Console.WriteLine(PhraseDictionary.Name);
        _phraseService = phraseService;
        LoadPhrasesForCurrentPage();
    }

    [RelayCommand]
    public void LoadPhrasesForCurrentPage()
    {
        if (!string.IsNullOrEmpty(SearchText))
        {
            SearchPhrases();
            return;
        }
        DisplayedPhrases.Clear();
        var phrasesInCurrentPage = _phraseService
            .GetPhrases(PhraseDictionary)
            .Paginate(CurrentPage,ElementsPerPage);
        TotalPages = _phraseService.GetPhrases(PhraseDictionary).GetTotalPages(ElementsPerPage);
        DisplayedPhrases.AddRange(phrasesInCurrentPage);
    }

    [RelayCommand]
    public void SearchPhrases()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            LoadPhrasesForCurrentPage();
            return;
        }
        DisplayedPhrases.Clear();
        var searchedPhrasesInCurrentPage = _phraseService.GetPhrases(PhraseDictionary)
            .Where(p => p.ToString().Contains(SearchText!, StringComparison.OrdinalIgnoreCase))
            .Paginate(CurrentPage, ElementsPerPage);
        TotalPages = _phraseService.GetPhrases(PhraseDictionary)
            .Where(p => p.ToString().Contains(SearchText!, StringComparison.OrdinalIgnoreCase))
            .GetTotalPages(ElementsPerPage);
        // CurrentPage = 1;
        DisplayedPhrases.AddRange(searchedPhrasesInCurrentPage);
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