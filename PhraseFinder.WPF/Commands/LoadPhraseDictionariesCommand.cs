using PhraseFinder.Data.Services;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Commands;

internal class LoadPhraseDictionariesCommand(
    PhraseDictionariesViewModel viewModel,
    IPhraseDictionaryService service) : CommandBase
{
    public override async void Execute(object? parameter)
    {
        var phraseDictionaries = await service.GetPhraseDictionariesAsync();
        foreach (var phraseDictionary in phraseDictionaries)
        {
            viewModel.PhraseDictionaries.Add(phraseDictionary);
        }
    }
}