using System.ComponentModel;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Commands;

public class AddPhraseDictionaryCommand : CommandBase
{
    private readonly AddPhraseDictionaryViewModel _viewModel;
    private readonly IPhraseDictionaryService _service;

    public AddPhraseDictionaryCommand(
        AddPhraseDictionaryViewModel viewModel,
        IPhraseDictionaryService service)
    {
        _viewModel = viewModel;
        _service = service;
        viewModel.PropertyChanged += RaiseCanExecuteChanged;
    }

    private void RaiseCanExecuteChanged(object? sender, PropertyChangedEventArgs args) 
    {
        if (args.PropertyName is nameof(_viewModel.PhraseDictionaryName)
            or nameof(_viewModel.PhraseDictionaryFilePath)
            or nameof(_viewModel.SelectedPhraseDictionaryFormat))
        {
            OnCanExecuteChanged();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_viewModel.PhraseDictionaryName) &&
               !string.IsNullOrWhiteSpace(_viewModel.PhraseDictionaryFilePath) &&
               _viewModel.SelectedPhraseDictionaryFormat != null;
    }

    public override async void Execute(object? parameter)
    {
        var phraseDictionary = new PhraseDictionary
        {
            Name = _viewModel.PhraseDictionaryName!,
            Format = (PhraseDictionaryFormat)_viewModel.SelectedPhraseDictionaryFormat!,
            Description = _viewModel.PhraseDictionaryDescription,
            FilePath = _viewModel.PhraseDictionaryFilePath!
        };
        await _service.AddPhraseDictionaryAsync(phraseDictionary);
    }
}