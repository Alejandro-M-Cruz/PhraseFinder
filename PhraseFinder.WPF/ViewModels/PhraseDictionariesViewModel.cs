using System.Collections.ObjectModel;
using System.Windows.Input;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Commands;

namespace PhraseFinder.WPF.ViewModels;

public class PhraseDictionariesViewModel : ViewModelBase
{
    private PhraseDictionary? _selectedPhraseDictionary;
    public PhraseDictionary? SelectedPhraseDictionary
    {
        get => _selectedPhraseDictionary;
        set => SetField(ref _selectedPhraseDictionary, value);
    }
    public ObservableCollection<PhraseDictionary> PhraseDictionaries { get; } = [];
    private LoadPhraseDictionariesCommand LoadPhraseDictionariesCommand { get; }
    public ICommand DeletePhraseDictionaryCommand { get; }
    public ICommand NavigateToPhrasesCommand { get; }
    public ICommand NavigateToAddPhraseDictionaryCommand { get; }

    public PhraseDictionariesViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        LoadPhraseDictionariesCommand = new LoadPhraseDictionariesCommand(this, phraseDictionaryService);
        LoadPhraseDictionariesCommand.Execute(null);
        DeletePhraseDictionaryCommand = new DeletePhraseDictionaryCommand(this, phraseDictionaryService);
        NavigateToPhrasesCommand = new NavigateToPhrasesCommand(this);
        NavigateToAddPhraseDictionaryCommand = new NavigateToAddPhraseDictionaryCommand();
    }
}