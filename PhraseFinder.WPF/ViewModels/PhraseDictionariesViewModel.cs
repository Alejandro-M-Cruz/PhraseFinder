using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
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
    private async Task LoadPhraseDictionaries()
    {
        var phraseDictionaries = 
            await _phraseDictionaryService.GetPhraseDictionariesAsync();
        foreach (var phraseDictionary in phraseDictionaries)
        {
            PhraseDictionaries.Add(phraseDictionary);
        }
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
        throw new NotImplementedException();
    }

    [RelayCommand]
    public void NavigateToAddPhraseDictionary()
    {
        _navigationService.NavigateTo<AddPhraseDictionaryViewModel>();
    }
}