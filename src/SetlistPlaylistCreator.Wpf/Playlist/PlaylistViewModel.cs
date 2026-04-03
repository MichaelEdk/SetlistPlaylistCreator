using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Playlist
{
    /// <summary>
    /// Represents the view model for managing a playlist, including creating and populating playlists based on a
    /// setlist. This class provides commands and properties to interact with the playlist data.
    /// </summary>
    /// <remarks>
    /// The <see cref="PlaylistViewModel"/> class is responsible for handling the logic related to
    /// playlist creation and management. It interacts with the <see cref="ISetlistPlaylistCreatorService"/> to propose
    /// and create playlists. The view model maintains a list of songs and their corresponding view models, allowing for
    /// easy data binding in a UI context.
    /// </remarks>
    public class PlaylistViewModel
        : ViewModelBase
    {
        private readonly ISetlistPlaylistCreatorService _setlistPlaylistCreatorService;

        private List<SearchedSong> _playlist = [];
        private List<PlaylistRowViewModel> _playlistRowViewModels = [];
        private string _setlistName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistViewModel"/> class.
        /// </summary>
        /// <param name="setlistPlaylistCreatorService">Handles interactions with the streaming platform related to playlists.</param>
        public PlaylistViewModel(ISetlistPlaylistCreatorService setlistPlaylistCreatorService)
        {
            ArgumentNullException.ThrowIfNull(setlistPlaylistCreatorService, nameof(setlistPlaylistCreatorService));

            _setlistPlaylistCreatorService = setlistPlaylistCreatorService;
        }

        /// <summary>
        /// An event raised when a playlist is successfully created.
        /// </summary>
        public event EventHandler? PlaylistCreated;

        /// <summary>
        /// Gets a command that creates a playlist based on the selected songs.
        /// </summary>
        public ICommand CreatePlaylist => new RelayCommand<object>(
            _ => SelectedSongs.Count != 0,
            async _ =>
            {
                if (await _setlistPlaylistCreatorService.CreatePlaylistAsync(_setlistName, SelectedSongs).ConfigureAwait(false))
                {
                    PlaylistCreated?.Invoke(this, EventArgs.Empty);
                }
            });

        /// <summary>
        /// Gets or sets the playlist of songs that have been searched on the streaming platform and selected for the playlist.
        /// </summary>
        public List<SearchedSong> Playlist
        {
            get => _playlist;
            set => RaiseAndSetIfChanged(ref _playlist, value, nameof(Playlist));
        }

        /// <summary>
        /// Gets or sets a collection of view models representing each row in the playlist. This is essentially a collection of songs.
        /// </summary>
        public List<PlaylistRowViewModel> PlaylistRowViewModels
        { 
            get => _playlistRowViewModels;
            set => RaiseAndSetIfChanged(ref _playlistRowViewModels, value, nameof(PlaylistRowViewModels));
        }

        private List<StreamingPlatformSong> SelectedSongs
        {
            get => PlaylistRowViewModels
                .Select(song => song.SelectedSong)
                .OfType<StreamingPlatformSong>() // Filter out nulls
                .ToList();
        }

        /// <summary>
        /// Asynchronously populates the setlist with proposed playlist items based on the provided setlist details.
        /// </summary>
        /// <remarks>This method updates the internal state with a list of playlist row view models, each
        /// representing a song proposed for the setlist. The method uses the setlist's name and song details to
        /// generate the playlist.</remarks>
        /// <param name="setlist">The setlist containing the details used to propose a playlist. Cannot be null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PopulateSetlistAsync(Setlist setlist)
        {
            ArgumentNullException.ThrowIfNull(setlist, nameof(setlist));

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
    }
}
