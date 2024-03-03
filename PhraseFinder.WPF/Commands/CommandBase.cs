using System.Windows.Input;

namespace PhraseFinder.WPF.Commands;

public abstract class CommandBase : ICommand
{
    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;

    public void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}