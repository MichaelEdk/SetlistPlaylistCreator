using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Complete
{
    /// <summary>
    /// A view model to back the CompleteView control. This is shown after the playlist has been created and allows the user to close the application.
    /// </summary>
    public class CompleteViewModel
        : ViewModelBase
    {
        /// <summary>
        /// An event raised when the user requests to shut down the application.
        /// </summary>
        public event EventHandler? ShutdownRequested;

        /// <summary>
        /// Gets a command that, when executed, will close the application.
        /// </summary>
        public ICommand CloseApplication => new RelayCommand<object>(_ => ShutdownRequested?.Invoke(this, EventArgs.Empty));
    }
}
