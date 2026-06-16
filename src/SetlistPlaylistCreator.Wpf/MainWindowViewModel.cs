using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using SetlistPlaylistCreator.Wpf.Complete;
using SetlistPlaylistCreator.Wpf.Playlist;
using SetlistPlaylistCreator.Wpf.SongList;
using Spotify.Client.Authorization;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// A view model backing the main window.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ArtistSearchViewModel _artistSearchViewModel;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly AuthorizationViewModel _authorizationViewModel;
        private readonly PlaylistViewModel _playlistViewModel;
        private readonly SongListViewModel _songListViewModel;
        private readonly CompleteViewModel _completeViewModel;

        private ViewModelBase? _previousViewModel;
        private ViewModelBase? _currentViewModel;
        private bool _canNavigateBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="authorizationViewModel">The view model for the authorization control.</param>
        /// <param name="artistSearchViewModel">The view model for searching for an artist.</param>
        /// <param name="songListViewModel">The view model for listing a setlist.</param>
        /// <param name="playlistViewModel">The view mode for showing a playlist.</param>
        /// <param name="completeViewModel">The view model for the completion screen.</param>
        /// <param name="authorizationCodeStore">Stores and retrieves the Spotify authorization code.</param>
        public MainWindowViewModel(
            AuthorizationViewModel authorizationViewModel,
            ArtistSearchViewModel artistSearchViewModel,
            SongListViewModel songListViewModel,
            PlaylistViewModel playlistViewModel,
            CompleteViewModel completeViewModel,
            IAuthorizationCodeStore authorizationCodeStore)
        {
            ArgumentNullException.ThrowIfNull(authorizationViewModel, nameof(authorizationViewModel));
            ArgumentNullException.ThrowIfNull(artistSearchViewModel, nameof(artistSearchViewModel));
            ArgumentNullException.ThrowIfNull(songListViewModel, nameof(songListViewModel));
            ArgumentNullException.ThrowIfNull(playlistViewModel, nameof(playlistViewModel));
            ArgumentNullException.ThrowIfNull(completeViewModel, nameof(completeViewModel));
            ArgumentNullException.ThrowIfNull(authorizationCodeStore, nameof(authorizationCodeStore));

            _authorizationViewModel = authorizationViewModel;
            _artistSearchViewModel = artistSearchViewModel;
            _songListViewModel = songListViewModel;
            _playlistViewModel = playlistViewModel;
            _completeViewModel = completeViewModel;
            _authorizationCodeStore = authorizationCodeStore;

            _authorizationViewModel.AuthorizationComplete += AuthorizationViewModel_AuthorizationComplete;
            _artistSearchViewModel.SetlistSelected += ArtistSearchViewModel_SetlistSelected;
            _songListViewModel.SearchSonglist += SongListViewModel_SearchSonglist;
            _playlistViewModel.PlaylistCreated += PlaylistViewModel_PlaylistCreated;
            _completeViewModel.ShutdownRequested += CompleteViewModel_ShutdownRequested;

            BackCommand = new RelayCommand<object>(_ => NavigateBack());
        }

        /// <summary>
        /// Gets the current view model being displayed.
        /// </summary>
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            private set => RaiseAndSetIfChanged(ref _currentViewModel, value, nameof(CurrentViewModel));
        }

        /// <summary>
        /// Gets a value indicating whether the user can navigate back to the previous view model.
        /// </summary>
        public bool CanNavigateBack
        {
            get => _canNavigateBack;
            private set => RaiseAndSetIfChanged(ref _canNavigateBack, value, nameof(CanNavigateBack));
        }

        /// <summary>
        /// Gets the command to navigate back to the previous view model.
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event with the initial view model based on the authorization code state.
        /// </summary>
        public void Initialize()
        {
            var authorizationCode = _authorizationCodeStore.RetrieveCode();

            ViewModelBase initialViewModel = string.IsNullOrWhiteSpace(authorizationCode) ?
                _authorizationViewModel :
                _artistSearchViewModel;

            NavigateTo(initialViewModel, trackPrevious: false);
        }

        /// <summary>
        /// Navigates back to the previous view model.
        /// </summary>
        private void NavigateBack()
        {
            if (_previousViewModel == null)
            {
                return;
            }

            var previousViewModel = _previousViewModel;
            _previousViewModel = null;
            NavigateTo(previousViewModel, trackPrevious: false);
        }

        /// <summary>
        /// Navigates to the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model to navigate to.</param>
        /// <param name="trackPrevious">Whether to track the current view model as the previous one.</param>
        private void NavigateTo(ViewModelBase viewModel, bool trackPrevious = true)
        {
            if (trackPrevious)
            {
                _previousViewModel = CurrentViewModel;
            }

            CurrentViewModel = viewModel;
            CanNavigateBack = _previousViewModel != null;
        }

        private void ArtistSearchViewModel_SetlistSelected(object? sender, SetlistSelectedEventArgs e)
        {
            _songListViewModel.Setlist = e.SelectedSetlist;
            NavigateTo(_songListViewModel);
        }

        private void AuthorizationViewModel_AuthorizationComplete(object? sender, EventArgs e)
        {
            NavigateTo(_artistSearchViewModel);
        }

        private void CompleteViewModel_ShutdownRequested(object? sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void PlaylistViewModel_PlaylistCreated(object? sender, EventArgs e)
        {
            NavigateTo(_completeViewModel);
        }

        private async void SongListViewModel_SearchSonglist(object? sender, SearchSongListEventArgs e)
        {
            _songListViewModel.IsSearching = true;

            try
            {
                await _playlistViewModel.PopulateSetlistAsync(e.SongList).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., show a message to the user
                // For now, we will just log it to the console
                Console.WriteLine($"Error populating setlist: {ex.Message}");
                return;
            }
            finally
            {
                _songListViewModel.IsSearching = false;
            }

            NavigateTo(_playlistViewModel);
        }
    }
}
