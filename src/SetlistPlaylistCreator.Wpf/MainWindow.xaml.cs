using System.Windows;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the view model of the main window and subscribes to appropriate events.
        /// </summary>
        /// <param name="viewModel">The view model to set.</param>
        public void SetMainWindowViewModel(MainWindowViewModel viewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.ContextChanged -= ViewModel_ContextChanged;
            }

            _viewModel = viewModel;
            _viewModel.ContextChanged += ViewModel_ContextChanged;
        }

        private void ViewModel_ContextChanged(object? sender, DataContextChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                DataContext = e.NewDataContext;
            }));
        }
    }
}