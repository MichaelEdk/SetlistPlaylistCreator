using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> that allows you to define an async action and a predicate to determine if the command can execute.
    /// </summary>
    /// <typeparam name="T">The type of the parameter passed into the command.</typeparam>
    public class AsyncRelayCommand<T>(Predicate<T?> canExecute, Func<T?, Task> asyncAction) : ICommand
        where T : class
    {
        private bool _isExecuting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="asyncAction">The async action to be executed when this command is used.</param>
        /// <remarks>CanExecute will always be true unless the command is currently executing.</remarks>
        public AsyncRelayCommand(Func<T?, Task> asyncAction)
            : this(_ => true, asyncAction) { }

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            if (_isExecuting)
            {
                return false;
            }

            var typedParameter = parameter as T;

            if (typedParameter == null && parameter != null)
            {
                // If the parameter is not of type T, we cannot execute the action.
                return false;
            }

            return canExecute == null || canExecute(typedParameter);
        }

        /// <inheritdoc />
        public async void Execute(object? parameter)
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

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await asyncAction(typedParameter).ConfigureAwait(true);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
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
