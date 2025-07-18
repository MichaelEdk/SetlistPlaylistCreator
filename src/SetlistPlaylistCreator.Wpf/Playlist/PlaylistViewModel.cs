using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Playlist
{
    public class PlaylistViewModel
        : ViewModelBase
    {
        private readonly ISetlistPlaylistCreatorService _setlistPlaylistCreatorService;

        private List<SearchedSong> _playlist = [];
        private string _setlistName = string.Empty;
        private List<PlaylistRowViewModel> _playlistRowViewModels = [];

        public PlaylistViewModel(ISetlistPlaylistCreatorService setlistPlaylistCreatorService)
        {
            ArgumentNullException.ThrowIfNull(setlistPlaylistCreatorService, nameof(setlistPlaylistCreatorService));

            _setlistPlaylistCreatorService = setlistPlaylistCreatorService;
        }

        public List<SearchedSong> Playlist
        {
            get => _playlist;
            set => RaiseAndSetIfChanged(ref _playlist, value, nameof(Playlist));
        }

        public List<PlaylistRowViewModel> PlaylistRowViewModels
        { 
            get => _playlistRowViewModels;
            set => RaiseAndSetIfChanged(ref _playlistRowViewModels, value, nameof(_playlistRowViewModels));
        }

        private List<StreamingPlatformSong> SelectedSongs
        {
            get => PlaylistRowViewModels
                .Select(song => song.SelectedSong)
                .OfType<StreamingPlatformSong>() // Filter out nulls
                .ToList();
        }

        public async Task PopulateSetlistAsync(Setlist setlist)
        {
            _setlistName = setlist.Name;
            var searchedSongs = await _setlistPlaylistCreatorService.ProposePlaylistAsync(setlist).ConfigureAwait(false);
            PlaylistRowViewModels = searchedSongs
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

        public ICommand CreatePlaylist => new RelayCommand<object>(async _ =>
        {
            await _setlistPlaylistCreatorService.CreatePlaylistAsync(_setlistName, SelectedSongs).ConfigureAwait(false);
        });
    }
}
