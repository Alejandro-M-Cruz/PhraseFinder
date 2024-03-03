﻿using System.ComponentModel;
using PhraseFinder.Data.Services;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Commands;

public class DeletePhraseDictionaryCommand : CommandBase
{
    private readonly PhraseDictionariesViewModel _viewModel;
    private readonly IPhraseDictionaryService _service;

    public DeletePhraseDictionaryCommand(
        PhraseDictionariesViewModel viewModel,
        IPhraseDictionaryService service)
    {
        _viewModel = viewModel;
        _service = service;
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
        _service.DeletePhraseDictionaryAsync(_viewModel.SelectedPhraseDictionary!);
    }
}