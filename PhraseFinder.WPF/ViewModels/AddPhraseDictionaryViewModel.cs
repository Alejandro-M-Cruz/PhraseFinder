using System.Windows.Input;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Commands;

namespace PhraseFinder.WPF.ViewModels;

public class AddPhraseDictionaryViewModel : ViewModelBase
{
    private string? _phraseDictionaryName = string.Empty;
    public string? PhraseDictionaryName
    {
        get => _phraseDictionaryName;
        set => SetField(ref _phraseDictionaryName, value);
    }

    private PhraseDictionaryFormat? _selectedPhraseDictionaryFormat;
    public PhraseDictionaryFormat? SelectedPhraseDictionaryFormat
    {
        get => _selectedPhraseDictionaryFormat;
        set => SetField(ref _selectedPhraseDictionaryFormat, value);
    }

    private string? _phraseDictionaryDescription;
    public string? PhraseDictionaryDescription
    {
        get => _phraseDictionaryDescription;
        set => SetField(ref _phraseDictionaryDescription, value);
    }

    private string? _phraseDictionaryFilePath;
    public string? PhraseDictionaryFilePath
    {
        get => _phraseDictionaryFilePath;
        set => SetField(ref _phraseDictionaryFilePath, value);
    }

    public ICommand AddPhraseDictionaryCommand { get; }
    public ICommand NavigateToPhraseDictionariesCommand { get; }

    public AddPhraseDictionaryViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        AddPhraseDictionaryCommand = new AddPhraseDictionaryCommand(this, phraseDictionaryService);
    }
}