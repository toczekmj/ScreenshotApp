using System.Windows.Input;

namespace ScreenshotTestApp.Commands;

public class AsyncRelayCommand(Func<object?, Task> executeAsync, Func<object?, bool>? canExecute = null) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute is null || canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        executeAsync(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}