using SetlistPlaylistCreator.Domain;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.SongList
{
    /// <summary>
    /// A view model to back the SongListUserControl view.
    /// </summary>
    public class SongListViewModel
        : ViewModelBase
    {
        private Domain.Setlist _setlist = new();

        /// <summary>
        /// An event raised when the user requests to search the streaming platform for songs in the setlist.
        /// </summary>
        public event EventHandler? SearchSonglist;

        /// <summary>
        /// Gets or sets the setlist to display in the song list view.
        /// </summary>
        public Setlist Setlist
        {
            get => _setlist;
            set => RaiseAndSetIfChanged(ref _setlist, value, nameof(Setlist));
        }

        /// <summary>
        /// Gets a command that searches the streaming platform for songs in the setlist.
        /// </summary>
        public ICommand SearchSpotify => new RelayCommand<object>(_ =>
        {
            SearchSonglist?.Invoke(this, EventArgs.Empty);
        });
    }
}
