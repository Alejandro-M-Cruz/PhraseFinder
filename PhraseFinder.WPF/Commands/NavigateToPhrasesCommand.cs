using System.ComponentModel;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Commands;

public class NavigateToPhrasesCommand : CommandBase
{
    private readonly PhraseDictionariesViewModel _viewModel;

    public NavigateToPhrasesCommand(PhraseDictionariesViewModel viewModel)
    {
        _viewModel = viewModel;
        viewModel.PropertyChanged += RaiseCanExecuteChanged;
    }
    
    private void RaiseCanExecuteChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(_viewModel.SelectedPhraseDictionary))
        {
            OnCanExecuteChanged();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return _viewModel.SelectedPhraseDictionary != null;
    }
    
    public override void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}