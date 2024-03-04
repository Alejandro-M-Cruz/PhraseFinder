using CommunityToolkit.Mvvm.ComponentModel;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.WPF.ViewModels;

public partial class PhraseDictionaryViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private PhraseDictionaryFormat? _format;

    [ObservableProperty]
    private string? _filePath;
}