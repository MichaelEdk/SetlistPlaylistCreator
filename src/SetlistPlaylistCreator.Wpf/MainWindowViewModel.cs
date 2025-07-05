using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using SetlistPlaylistCreator.Wpf.Playlist;
using SetlistPlaylistCreator.Wpf.SongList;
using Spotify.Client.Authorization;

namespace SetlistPlaylistCreator.Wpf
{
    public class MainWindowViewModel
    {
        private readonly AuthorizationViewModel _authorizationViewModel;
        private readonly ArtistSearchViewModel _artistSearchViewModel;
        private readonly SongListViewModel _songListViewModel;
        private readonly PlaylistViewModel _playlistViewModel;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;

        public event EventHandler<DataContextChangedEventArgs>? ContextChanged;

        public MainWindowViewModel(
            AuthorizationViewModel authorizationViewModel,
            ArtistSearchViewModel artistSearchViewModel,
            SongListViewModel songListViewModel,
            PlaylistViewModel playlistViewModel,
            IAuthorizationCodeStore authorizationCodeStore)
        {
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
            await _playlistViewModel.PopulateSetlistAsync(_songListViewModel.Setlist).ConfigureAwait(true);
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
