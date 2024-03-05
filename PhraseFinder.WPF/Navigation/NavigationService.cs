using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PhraseFinder.WPF.Navigation;

internal partial class NavigationService(
    Func<Type, INotifyPropertyChanged> viewModelFactory) : ObservableObject, INavigationService
{
    [ObservableProperty]
    private INotifyPropertyChanged? _currentViewModel;

    public void NavigateTo<TViewModel>() where TViewModel : INotifyPropertyChanged
    {
        CurrentViewModel = viewModelFactory.Invoke(typeof(TViewModel));
    }
}