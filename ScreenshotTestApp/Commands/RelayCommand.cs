using System.Windows.Input;

namespace ScreenshotTestApp.Commands;

public class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object? parameter)
    {
        execute(parameter);
    }
    
    public bool CanExecute(object? parameter)
    {
        return canExecute is null || canExecute(parameter);
    }
}
