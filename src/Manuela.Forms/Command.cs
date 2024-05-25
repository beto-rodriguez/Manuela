using System.Windows.Input;

namespace Manuela.Forms;

public class Command(Action<object?> predicate) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        predicate(parameter);
    }
}
