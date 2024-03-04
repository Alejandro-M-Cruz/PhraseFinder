using CommunityToolkit.Mvvm.ComponentModel;
using PhraseFinder.Data.Services;

namespace PhraseFinder.WPF.ViewModels;

public class MainViewModel : ObservableObject
{
    public ObservableObject CurrentViewModel { get; }

    public MainViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        CurrentViewModel = new PhraseDictionariesViewModel(phraseDictionaryService);
        //CurrentViewModel = new AddPhraseDictionaryViewModel(phraseDictionaryService);
    }
}