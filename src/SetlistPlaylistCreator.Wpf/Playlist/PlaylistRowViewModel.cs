using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Playlist
{
    /// <summary>
    /// Represents a row in the playlist view model.
    /// </summary>
    public class PlaylistRowViewModel
        : ViewModelBase
    {
        private bool _isPopupVisible = false;
        private SearchedSong? _searchedSong;
        private StreamingPlatformSong? _selectedSong;
        private string _setlistArtist = string.Empty;
        private string _setlistSongName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistRowViewModel"/> class.
        /// </summary>
        public PlaylistRowViewModel()
        {
            SelectSongPopup = new RelayCommand<StreamingPlatformSong>(song =>
            {
                SelectedSong = song;
                IsPopupVisible = false;
            });
        }

        /// <summary>
        /// Shows the popup allowing the user to edit their song choice for the playlist row.
        /// </summary>
        public ICommand EditSongChoice => new RelayCommand<object>(_ =>
        {
            IsPopupVisible = true;
        });

        /// <summary>
        /// Gets or sets a unique identifier for the playlist row.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the popup for editing song choices is visible.
        /// </summary>
        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set => RaiseAndSetIfChanged(ref _isPopupVisible, value, nameof(IsPopupVisible));
        }

        /// <summary>
        /// Gets or sets the song search results.
        /// </summary>
        public SearchedSong? SearchedSong
        {
            get => _searchedSong;
            set => RaiseAndSetIfChanged(ref _searchedSong, value, nameof(SearchedSong));
        }

        /// <summary>
        /// Gets or sets the streaming platform song that was selected by the user.
        /// </summary>
        /// <remarks>This should generally default to the first match of the searched song.</remarks>
        public StreamingPlatformSong? SelectedSong
        {
            get => _selectedSong;
            set => RaiseAndSetIfChanged(ref _selectedSong, value, nameof(SelectedSong));
        }

        /// <summary>
        /// A command that allows the user to select a song from the popup.
        /// </summary>
        public ICommand SelectSongPopup { get; }

        /// <summary>
        /// Gets or sets the artist name from the setlist.
        /// </summary>
        /// <remarks>This is the song artist, not the setlist artist.</remarks>
        public string SetlistArtist
        {
            get => _setlistArtist;
            set => RaiseAndSetIfChanged(ref _setlistArtist, value, nameof(SetlistArtist));
        }

        /// <summary>
        /// Gets or sets the name of the song from the setlist.
        /// </summary>
        public string SetlistSongName
        {
            get => _setlistSongName;
            set => RaiseAndSetIfChanged(ref _setlistSongName, value, nameof(SetlistSongName));
        }
    }
}
