using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using SetlistPlaylistCreator.Wpf.Playlist;
using SetlistPlaylistCreator.Wpf.SongList;
using Spotify.Client.Authorization;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// A view model backing the main window.
    /// </summary>
    public class MainWindowViewModel
    {
        private readonly AuthorizationViewModel _authorizationViewModel;
        private readonly ArtistSearchViewModel _artistSearchViewModel;
        private readonly SongListViewModel _songListViewModel;
        private readonly PlaylistViewModel _playlistViewModel;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;

        /// <summary>
        /// An event raised when the data context has changed. The user control displayed in the main
        /// window is dependent on the view model.
        /// </summary>
        public event EventHandler<DataContextChangedEventArgs>? ContextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="authorizationViewModel">The view model for the authorization control.</param>
        /// <param name="artistSearchViewModel">The view model for searching for an artist.</param>
        /// <param name="songListViewModel">The view model for listing a setlist.</param>
        /// <param name="playlistViewModel">The view mode for showing a playlist.</param>
        /// <param name="authorizationCodeStore">Stores and retrieves the Spotify authorization code.</param>
        public MainWindowViewModel(
            AuthorizationViewModel authorizationViewModel,
            ArtistSearchViewModel artistSearchViewModel,
            SongListViewModel songListViewModel,
            PlaylistViewModel playlistViewModel,
            IAuthorizationCodeStore authorizationCodeStore)
        {
            ArgumentNullException.ThrowIfNull(authorizationViewModel, nameof(authorizationViewModel));
            ArgumentNullException.ThrowIfNull(artistSearchViewModel, nameof(artistSearchViewModel));
            ArgumentNullException.ThrowIfNull(songListViewModel, nameof(songListViewModel));
            ArgumentNullException.ThrowIfNull(playlistViewModel, nameof(playlistViewModel));
            ArgumentNullException.ThrowIfNull(authorizationCodeStore, nameof(authorizationCodeStore));

            _authorizationViewModel = authorizationViewModel;
            _artistSearchViewModel = artistSearchViewModel;
            _songListViewModel = songListViewModel;
            _playlistViewModel = playlistViewModel;
            _authorizationCodeStore = authorizationCodeStore;

            _authorizationViewModel.AuthorizationComplete += AuthorizationViewModel_AuthorizationComplete;
            _artistSearchViewModel.SetlistSelected += ArtistSearchViewModel_SetlistSelected;
            _songListViewModel.SearchSonglist += SongListViewModel_SearchSonglist;
        }

        private async void SongListViewModel_SearchSonglist(object? sender, EventArgs e)
        {
            try
            {
                if (_songListViewModel.Setlist == null)
                {
                    // Handle the case where no setlist is populated
                    Console.WriteLine("No setlist populated.");
                    return;
                }

                await _playlistViewModel.PopulateSetlistAsync(_songListViewModel.Setlist).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., show a message to the user
                // For now, we will just log it to the console
                Console.WriteLine($"Error populating setlist: {ex.Message}");
                return;
            }
            ContextChanged?.Invoke(this, new DataContextChangedEventArgs(_playlistViewModel));
        }

        private void ArtistSearchViewModel_SetlistSelected(object? sender, EventArgs e)
        {
            _songListViewModel.Setlist = _artistSearchViewModel.SelectedSetlist;
            ContextChanged?.Invoke(this, new DataContextChangedEventArgs(_songListViewModel));
        }

        public void Initialize()
        {
            var authorizationCode = _authorizationCodeStore.RetrieveCode();

            var initialViewModel = string.IsNullOrWhiteSpace(authorizationCode) ?
                new DataContextChangedEventArgs(_authorizationViewModel) :
                new DataContextChangedEventArgs(_artistSearchViewModel);

            ContextChanged?.Invoke(this, initialViewModel);
        }

        private void AuthorizationViewModel_AuthorizationComplete(object? sender, EventArgs e)
        {
            ContextChanged?.Invoke(this, new DataContextChangedEventArgs(_artistSearchViewModel));
        }
    }
}
