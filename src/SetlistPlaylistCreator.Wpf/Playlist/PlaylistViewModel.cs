using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Playlist
{
    public class PlaylistViewModel
        : ViewModelBase
    {
        private readonly ISetlistPlaylistCreatorService _setlistPlaylistCreatorService;

        private IReadOnlyCollection<SearchedSong>? _playlist;
        private string? _setlistName;
        private IReadOnlyCollection<PlaylistRowViewModel> _playlistRowViewModel;

        public PlaylistViewModel(ISetlistPlaylistCreatorService setlistPlaylistCreatorService)
        {
            ArgumentNullException.ThrowIfNull(setlistPlaylistCreatorService, nameof(setlistPlaylistCreatorService));

            _setlistPlaylistCreatorService = setlistPlaylistCreatorService;
        }

        public IReadOnlyCollection<SearchedSong>? Playlist
        {
            get => _playlist;
            set => RaiseAndSetIfChanged(ref _playlist, value, nameof(Playlist));
        }

        public IReadOnlyCollection<PlaylistRowViewModel> PlaylistRowViewModel
        { 
            get => _playlistRowViewModel;
            set => RaiseAndSetIfChanged(ref _playlistRowViewModel, value, nameof(_playlistRowViewModel));
        }

        public async Task PopulateSetlistAsync(Setlist setlist)
        {
            _setlistName = setlist.Name;
            var searchedSongs = await _setlistPlaylistCreatorService.ProposePlaylistAsync(setlist).ConfigureAwait(false);
            PlaylistRowViewModel = searchedSongs
                .Select(song =>
                    new PlaylistRowViewModel()
                    {
                        Id = Guid.NewGuid(),
                        SearchedSong = song,
                        SelectedSong = song.FirstMatch,
                        SetlistArtist = song.SearchedArtistName,
                        SetlistSongName = song.SearchedSongName
                    }).ToList();
        }


        public ICommand EditSongChoice => new RelayCommand<PlaylistRowViewModel>(async row =>
        {
            
        });

        public ICommand CreatePlaylist => new RelayCommand<object>(async _ =>
        {
            await _setlistPlaylistCreatorService.CreatePlaylistAsync(_setlistName!, PlaylistRowViewModel!.Select(song => song.SelectedSong).ToList()).ConfigureAwait(false);
        });
    }
}
