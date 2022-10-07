using System;
using System.Windows.Input;
using FW.WPF.Commands.Base;

namespace FW.WPF.Commands;

public class LambdaCommand : Command
{
    private readonly Action<object?> _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommand(Action Execute, Func<bool>? CanExecute = null)
        :this(_ => Execute(), CanExecute is null ? null : _ => CanExecute()) { }

    public LambdaCommand(Action<object?> Execute, Func<object?, bool>? CanExecute = null)
    {
        _Execute = Execute;
        _CanExecute = CanExecute;
    }

    public override bool CanExecute(object? parameter) => App.IsDesignMode ||
        base.CanExecute(parameter) && (_CanExecute?.Invoke(parameter) ?? true);

    public override void Execute(object? parameter)
    {
        if(!CanExecute(parameter)) return;
        _Execute(parameter);
    }
}
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null)
            throw new ArgumentNullException("execute");
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null ? true : _canExecute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}
