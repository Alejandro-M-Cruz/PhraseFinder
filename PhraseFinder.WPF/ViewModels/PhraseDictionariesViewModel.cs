﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Messages;
using PhraseFinder.WPF.Navigation;
using PhraseFinder.WPF.Views;

namespace PhraseFinder.WPF.ViewModels;

internal partial class PhraseDictionariesViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(DisplayDeleteConfirmationDialogCommand), 
        nameof(NavigateToPhrasesCommand))]
    private PhraseDictionary? _selectedPhraseDictionary;

    public bool IsPhraseDictionarySelected() => SelectedPhraseDictionary != null;

    public ObservableCollection<PhraseDictionary> PhraseDictionaries { get; } = [];

    private readonly IPhraseDictionaryService _phraseDictionaryService;

    private readonly INavigationService _navigationService;

    public PhraseDictionariesViewModel(
        IPhraseDictionaryService phraseDictionaryService,
        INavigationService navigationService)
    {
        _phraseDictionaryService = phraseDictionaryService;
        _navigationService = navigationService;
        LoadPhraseDictionariesCommand.Execute(null);
    }

    [RelayCommand]
    public async Task LoadPhraseDictionaries()
    {
        var phraseDictionaries = 
            await _phraseDictionaryService.GetPhraseDictionariesAsync();
        PhraseDictionaries.AddRange(phraseDictionaries);
    }

    private Dialog? _deleteConfirmationDialog;

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public void DisplayDeleteConfirmationDialog()
    {
        _deleteConfirmationDialog = Dialog.Show(new DeleteConfirmationDialog()
        {
            Title = "¿Está seguro de que desea eliminar este diccionario? " +
                    "Se borrarán las expresiones y locuciones que contiene.",
            ConfirmButtonCommand = DeletePhraseDictionaryCommand
        });
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public async Task DeletePhraseDictionary()
    {
        _deleteConfirmationDialog?.Close();
        await _phraseDictionaryService.DeletePhraseDictionaryAsync(SelectedPhraseDictionary!);
        PhraseDictionaries.Remove(SelectedPhraseDictionary!);
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public void NavigateToPhrases()
    {
        WeakReferenceMessenger
            .Default
            .Register<PhraseDictionariesViewModel, PhraseDictionaryRequestMessage>(
                this, 
                (recipient, message) =>
                {
                    message.Reply(recipient.SelectedPhraseDictionary!);
                });
        _navigationService.NavigateTo<PhrasesViewModel>();
    }

    [RelayCommand]
    public void NavigateToAddPhraseDictionary()
    {
        _navigationService.NavigateTo<AddPhraseDictionaryViewModel>();
    }
}