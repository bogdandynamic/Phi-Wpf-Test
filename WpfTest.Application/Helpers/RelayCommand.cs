using System;
using System.Windows.Input;

namespace WpfTest.Application.Helpers;

/// <summary>
/// Relay Command implementation
/// </summary>
/// <typeparam name="T">Generic parameter for Relay Command</typeparam>
public class RelayCommand<T> : ICommand
{
	private readonly Predicate<T?>? _canExecute;
	private readonly Action<T?>? _execute;

	/// <summary>
	/// Constructor with execute param
	/// </summary>
	/// <param name="execute">The execute action (concrete implementation)</param>
	public RelayCommand(Action<T?> execute)
		: this(execute, null)
	{
		_execute = execute;
	}
 
	/// <summary>
	/// Constructor with execute and canExecute params
	/// </summary>
	/// <param name="execute">The execute action (concrete implementation)</param>
	/// <param name="canExecute">The canExecute predicate (concrete implementation)</param>
	/// <exception cref="ArgumentNullException"></exception>
	public RelayCommand(Action<T?>? execute, Predicate<T?>? canExecute)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}
 
	/// <summary>
	/// Method to determine if the action can be executed
	/// </summary>
	/// <param name="parameter">The parameter for the canExecute predicate</param>
	/// <returns>The result of the predicate evaluation</returns>
	public bool CanExecute(object? parameter) => _canExecute is null || _canExecute((T?)parameter);

	/// <summary>
	/// Method to execute the action
	/// </summary>
	/// <param name="parameter">The parameter for the execute action</param>
	public void Execute(object? parameter) => _execute?.Invoke((T?)parameter);

	/// <summary>
	/// Event handler for the changes of canExecute
	/// </summary>
	public event EventHandler? CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}
}

/// <summary>
/// Default Relay Command implementation with object as generic
/// </summary>
public class RelayCommand : RelayCommand<object>
{
	/// <inheritdoc />
	public RelayCommand(Action<object?> execute) : base(execute) { }

	/// <inheritdoc />
	public RelayCommand(Action<object?>? execute, Predicate<object?>? canExecute) : base(execute, canExecute) { }
}