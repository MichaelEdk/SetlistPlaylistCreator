using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf
{
    internal class RelayCommand<T> : ICommand
        where T : class
    {
        private readonly Action<T> _action;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Predicate<T> canExecute, Action<T> action)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<T> action)
            : this(_ => true, action) { }


        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return _canExecute == null || _canExecute(parameter as T);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public void Execute(object? parameter)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            _action?.Invoke(parameter as T);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
