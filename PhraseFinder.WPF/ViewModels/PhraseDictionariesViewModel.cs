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
        set
        {
            SetField(ref _selectedPhraseDictionary, value);
            DeletePhraseDictionaryCommand.OnCanExecuteChanged();
        }
    }

    public ObservableCollection<PhraseDictionary> PhraseDictionaries { get; } = [];

    public ICommand LoadPhraseDictionariesCommand { get; }
    public CommandBase DeletePhraseDictionaryCommand { get; }

    public PhraseDictionariesViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        DeletePhraseDictionaryCommand = 
            new DeletePhraseDictionaryCommand(this, phraseDictionaryService);
        LoadPhraseDictionariesCommand = 
            new LoadPhraseDictionariesCommand(this, phraseDictionaryService);
        LoadPhraseDictionariesCommand.Execute(null);
    }
}