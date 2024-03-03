using PhraseFinder.Data.Services;

namespace PhraseFinder.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; }

    public MainViewModel(IPhraseDictionaryService phraseDictionaryService)
    {
        CurrentViewModel = new PhraseDictionariesViewModel(phraseDictionaryService);
    }
}