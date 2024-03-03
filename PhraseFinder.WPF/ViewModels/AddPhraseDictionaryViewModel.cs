using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhraseFinder.Domain.Models;

namespace PhraseFinder.WPF.ViewModels;

public class AddPhraseDictionaryViewModel : ViewModelBase
{
    private string? _phraseDictionaryName = string.Empty;
    public string? PhraseDictionaryName
    {
        get => _phraseDictionaryName;
        set => SetField(ref _phraseDictionaryName, value);
    }

    private string? _phraseDictionaryDescription;
    public string? PhraseDictionaryDescription
    {
        get => _phraseDictionaryDescription;
        set => SetField(ref _phraseDictionaryDescription, value);
    }

    private string? _phraseDictionaryFilePath;
    public string? PhraseDictionaryFilePath
    {
        get => _phraseDictionaryFilePath;
        set => SetField(ref _phraseDictionaryFilePath, value);
    }

}