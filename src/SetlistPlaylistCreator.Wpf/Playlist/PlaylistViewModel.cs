using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Playlist
{
    public class PlaylistViewModel : ViewModelBase
    {
        private readonly ISetlistPlaylistCreatorService _setlistPlaylistCreatorService;

        private IReadOnlyCollection<StreamingPlatformSong> _playlist;
        private string _setlistName;

        public PlaylistViewModel(ISetlistPlaylistCreatorService setlistPlaylistCreatorService)
        {
            _setlistPlaylistCreatorService = setlistPlaylistCreatorService;
        }

        public IReadOnlyCollection<StreamingPlatformSong> Playlist
        {
            get => _playlist;
            set => RaiseAndSetIfChanged(ref _playlist, value, nameof(Playlist));
        }

        public async Task PopulateSetlistAsync(Setlist setlist)
        {
            _setlistName = setlist.Name;
            Playlist = await _setlistPlaylistCreatorService.CreateProposedPlaylistAsync(setlist).ConfigureAwait(false);
        }

        public ICommand CreatePlaylist => new RelayCommand<object>(async _ =>
        {
            await _setlistPlaylistCreatorService.CreatePlaylistAsync(_setlistName, Playlist).ConfigureAwait(false);
        });
    }
}
