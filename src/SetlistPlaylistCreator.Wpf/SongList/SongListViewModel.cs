using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.Service;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.SongList
{
    public class SongListViewModel : ViewModelBase
    {
        private Domain.Setlist _setlist;

        public event EventHandler SearchSonglist;

        public Setlist Setlist
        {
            get => _setlist;
            set => RaiseAndSetIfChanged(ref _setlist, value, nameof(Setlist));
        }

        public ICommand SearchSpotify => new RelayCommand<object>(_ =>
        {
            SearchSonglist?.Invoke(this, EventArgs.Empty);
        });
    }
}
