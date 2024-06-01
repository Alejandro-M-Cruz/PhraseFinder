using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PhraseFinder.Data.Services;
using PhraseFinder.Domain.Models;
using PhraseFinder.WPF.Navigation;

namespace PhraseFinder.WPF.ViewModels;

internal partial class AddPhraseDictionaryViewModel(
    IPhraseDictionaryService phraseDictionaryService,
    INavigationService navigationService) : ObservableObject
{
    [ObservableProperty] 
    private bool _displayErrorMessage;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPhraseDictionaryCommand))]
    private string? _phraseDictionaryName = string.Empty;

    [ObservableProperty] 
    private string? _phraseDictionaryDescription;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddPhraseDictionaryCommand))]
    private PhraseDictionaryFormat? _selectedPhraseDictionaryFormat = PhraseDictionaryFormat.DleTxt;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(
        nameof(AddPhraseDictionaryCommand),
        nameof(PickPhraseDictionaryFileCommand),
        nameof(UnpickPhraseDictionaryFileCommand))]
    private string? _phraseDictionaryFilePath;

    [ObservableProperty]
    private bool _isDictionaryBeingAdded;

    private bool IsPhraseDictionaryFilePicked => PhraseDictionaryFilePath != null;

    private bool IsPhraseDictionaryFileNotPicked => PhraseDictionaryFilePath == null;

    public bool CanAddPhraseDictionary =>
        !string.IsNullOrWhiteSpace(PhraseDictionaryName) &&
        !string.IsNullOrWhiteSpace(PhraseDictionaryFilePath) &&
        SelectedPhraseDictionaryFormat != null;

    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _addPhraseDictionaryTask;

    [RelayCommand(CanExecute = nameof(CanAddPhraseDictionary))]
    public async Task AddPhraseDictionary()
    {
        DisplayErrorMessage = false;
        var phraseDictionary = new PhraseDictionary
        {
            Name = PhraseDictionaryName!,
            Format = (PhraseDictionaryFormat)SelectedPhraseDictionaryFormat!,
            Description = PhraseDictionaryDescription,
            FilePath = PhraseDictionaryFilePath!
		};
        IsDictionaryBeingAdded = true;
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        try
        {
            _addPhraseDictionaryTask = Task.Run(
                () => phraseDictionaryService.AddPhraseDictionaryFromFileAsync(phraseDictionary, token),
                token);
            await _addPhraseDictionaryTask;
        }
        catch (OperationCanceledException)
        { }
        catch
        {
            DisplayErrorMessage = true;
        }
        finally
        {
            _addPhraseDictionaryTask = null;
	        IsDictionaryBeingAdded = false;
            if (!DisplayErrorMessage)
            {
	            navigationService.NavigateTo<PhraseDictionariesViewModel>();
            }
        }
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionaryFileNotPicked))]
    public void PickPhraseDictionaryFile()
    {
        OpenFileDialog openFileDialog = new() { Multiselect = false };
        if (openFileDialog.ShowDialog() == true)
        {
            PhraseDictionaryFilePath = openFileDialog.FileName;
        }
    }

    [RelayCommand(CanExecute = nameof(IsPhraseDictionaryFilePicked))]
    public void UnpickPhraseDictionaryFile()
    {
        PhraseDictionaryFilePath = null;
    }

    [RelayCommand]
    public async Task Cancel()
    {
	    if (_cancellationTokenSource != null)
	    {
			await _cancellationTokenSource.CancelAsync();
		}
		if (_addPhraseDictionaryTask != null)
		{
			try
			{
				await _addPhraseDictionaryTask;
			}
			catch (OperationCanceledException)
			{
				
			}
		}
		navigationService.NavigateTo<PhraseDictionariesViewModel>();
    }
}
