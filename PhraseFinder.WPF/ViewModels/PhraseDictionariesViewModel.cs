﻿using System.Collections.ObjectModel;
using System.Windows.Controls;
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

internal partial class PhraseDictionariesViewModel : 
    ObservableObject, IRecipient<PhraseDictionaryRequestMessage>
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
	    nameof(UpdatePhraseDictionaryCommand),
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
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(PhraseDictionaryRequestMessage message)
    {
        if (!message.HasReceivedResponse)
        {
            message.Reply(SelectedPhraseDictionary!);
        }

        WeakReferenceMessenger.Default
            .Unregister<PhraseDictionaryRequestMessage>(this);
    }

    [RelayCommand]
    public async Task LoadPhraseDictionaries()
    {
        var phraseDictionaries = 
            await _phraseDictionaryService.GetPhraseDictionariesAsync();
        PhraseDictionaries.AddRange(phraseDictionaries);
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public async Task UpdatePhraseDictionary(DataGridRowEditEndingEventArgs e)
    {
	    if (e.EditAction == DataGridEditAction.Cancel)
	    {
		    return;
	    }
		var phraseDictionary = (PhraseDictionary)e.Row.Item;
		await _phraseDictionaryService.UpdatePhraseDictionaryAsync(phraseDictionary);
	}

	private Dialog? _deleteConfirmationDialog;

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public void DisplayDeleteConfirmationDialog()
    {
        _deleteConfirmationDialog = Dialog.Show(new DeleteConfirmationDialog
        {
            Title = "¿Está seguro de que desea eliminar este diccionario?",
            Message = "Se eliminarán todas las expresiones y locuciones que contiene.",
			ConfirmButtonCommand = DeletePhraseDictionaryCommand
        });
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public async Task DeletePhraseDictionary()
    {
        await Task.Run(() => 
	        _phraseDictionaryService.DeletePhraseDictionaryAsync(SelectedPhraseDictionary!));
        PhraseDictionaries.Remove(SelectedPhraseDictionary!);
        _deleteConfirmationDialog?.Close();
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionarySelected))]
    public void NavigateToPhrases()
    {
        _navigationService.NavigateTo<PhrasesViewModel>();
    }

    [RelayCommand]
    public void NavigateToAddPhraseDictionary()
    {
        _navigationService.NavigateTo<AddPhraseDictionaryViewModel>();
    }
}