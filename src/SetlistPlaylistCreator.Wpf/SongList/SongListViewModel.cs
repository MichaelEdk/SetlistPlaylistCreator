using SetlistPlaylistCreator.Domain;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.SongList
{
    /// <summary>
    /// A view model to back the SongListUserControl view. On this page, the user can view a setlist and search for songs on a streaming platform.
    /// </summary>
    public class SongListViewModel
        : ViewModelBase
    {
        private bool _isSearching;
        private Setlist? _setlist;

        /// <summary>
        /// An event raised when the user requests to search the streaming platform for songs in the setlist.
        /// </summary>
        public event EventHandler<SearchSongListEventArgs>? SearchSonglist;

        /// <summary>
        /// Gets or sets the setlist to display in the song list view.
        /// </summary>
        public Setlist? Setlist
        {
            get => _setlist;
            set => RaiseAndSetIfChanged(ref _setlist, value, nameof(Setlist));
        }

        /// <summary>
        /// Gets or sets a value indicating whether a Spotify search is currently in progress.
        /// </summary>
        public bool IsSearching
        {
            get => _isSearching;
            set => RaiseAndSetIfChanged(ref _isSearching, value, nameof(IsSearching));
        }

        /// <summary>
        /// Gets a command that searches the streaming platform for songs in the setlist.
        /// </summary>
        public ICommand SearchSpotify => new RelayCommand<object>(
            _ => Setlist is not null && !IsSearching,
            _ =>
            {
                SearchSonglist?.Invoke(this, new SearchSongListEventArgs(Setlist!));
            });
    }
}
