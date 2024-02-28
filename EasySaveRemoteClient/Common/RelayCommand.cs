using System.Windows.Input;

namespace EasySaveRemoteClient.Common;

public class RelayCommand : ICommand
{
    private readonly Action execute;

    private readonly Func<bool>? canExecute;

    public RelayCommand(Action execute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        this.execute = execute;
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke() != false;
    }

    public void Execute(object? parameter)
    {
        execute();
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}