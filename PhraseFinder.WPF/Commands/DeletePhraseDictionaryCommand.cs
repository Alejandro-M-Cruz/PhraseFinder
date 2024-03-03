using PhraseFinder.Data.Services;
using PhraseFinder.WPF.ViewModels;

namespace PhraseFinder.WPF.Commands;

class DeletePhraseDictionaryCommand(
    PhraseDictionariesViewModel viewModel,
    IPhraseDictionaryService service) : CommandBase
{
    public override bool CanExecute(object? parameter)
    {
        return viewModel.SelectedPhraseDictionary != null;
    }

    public override void Execute(object? parameter)
    {
        service.DeletePhraseDictionaryAsync(viewModel.SelectedPhraseDictionary!);
    }
}