using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    ///  An implemntation of <see cref="ICommand"/> that allows you to define an action and a predicate to determine if the command can execute.
    /// </summary>
    /// <typeparam name="T">The type of the parameter passed into the command.</typeparam>
    public class RelayCommand<T>(Predicate<T?> canExecute, Action<T?> action) : ICommand
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="action">The action to be executed when this command is used.</param>
        /// <remarks>CanExecute will always be true.</remarks>
        public RelayCommand(Action<T?> action)
            : this(_ => true, action) { }

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            var typedParameter = parameter as T;

            if (typedParameter == null && parameter != null)
            {
                // If the parameter is not of type T, we cannot execute the action.
                return false;
            }

            return canExecute == null || canExecute(typedParameter);
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            var typedParameter = parameter as T;

            if (typedParameter == null && parameter != null)
            {
                // If the parameter is null or not of type T, we cannot execute the action.
                return;
            }

            if (!CanExecute(typedParameter))
            {
                // If the command cannot execute, we do not invoke the action.
                return;
            }

            action?.Invoke(typedParameter);
        }

        /// <summary>
        /// Invalidates the command's CanExecute state, causing it to re-evaluate whether it can execute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
