using CommunityToolkit.Mvvm.ComponentModel;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private INavigationService _navigation;

    public MainViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        _navigation.NavigateTo<PhraseDictionariesViewModel>();
    }
}