using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.SetlistPlatform;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.ArtistSearch
{
    public class ArtistSearchViewModel : ViewModelBase
    {
        private readonly ISetlistSearch _setlistSearch;

        public event EventHandler SetlistSelected;

        public ArtistSearchViewModel(ISetlistSearch setlistSearch)
        {
            _setlistSearch = setlistSearch;
        }

        private string? _artistSearchTerm;
        private List<Domain.Setlist> _setlists = new();

        public string? ArtistSearchTerm
        {
            get => _artistSearchTerm;
            set => RaiseAndSetIfChanged(ref _artistSearchTerm, value, nameof(ArtistSearchTerm));
        }

        public List<Domain.Setlist> Setlists
        {
            get => _setlists;
            set => RaiseAndSetIfChanged(ref _setlists, value, nameof(Setlists));
        }

        public Setlist SelectedSetlist { get; set; }

        public ICommand SearchArtists => new RelayCommand<object>(_ =>
        {
            Setlists = _setlistSearch.SearchForSetlistsAsync(ArtistSearchTerm, CancellationToken.None).GetAwaiter().GetResult().ToList();
        });

        public ICommand SelectSetlist => new RelayCommand<Domain.Setlist>(setlist =>
        {
            SelectedSetlist = setlist;
            SetlistSelected?.Invoke(this, EventArgs.Empty);
        });
    }
}
