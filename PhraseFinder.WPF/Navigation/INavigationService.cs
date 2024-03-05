using System.ComponentModel;

namespace PhraseFinder.WPF.Navigation;

internal interface INavigationService
{
    public INotifyPropertyChanged? CurrentViewModel { get; }

    public void NavigateTo<TViewModel>() where TViewModel : INotifyPropertyChanged;
}