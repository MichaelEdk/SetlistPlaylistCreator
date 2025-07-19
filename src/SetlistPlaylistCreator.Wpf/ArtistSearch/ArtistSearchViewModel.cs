using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.SetlistPlatform;

namespace SetlistPlaylistCreator.Wpf.ArtistSearch
{
    /// <summary>
    /// A view model for searching for artists and displaying their setlists.
    /// </summary>
    public class ArtistSearchViewModel : ViewModelBase
    {
        private readonly ISetlistSearch _setlistSearch;

        private string _artistSearchTerm = string.Empty;
        private List<Setlist> _setlists = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistSearchViewModel"/> class.
        /// </summary>
        /// <param name="setlistSearch">Component used for searching for setlists.</param>
        public ArtistSearchViewModel(ISetlistSearch setlistSearch)
        {
            ArgumentNullException.ThrowIfNull(setlistSearch, nameof(setlistSearch));

            _setlistSearch = setlistSearch;
        }

        /// <summary>
        /// An event that is raised when a setlist is selected.
        /// </summary>
        public event EventHandler<SetlistSelectedEventArgs>? SetlistSelected;

        /// <summary>
        /// Gets or sets the search term used to search for an artist's setlists.
        /// </summary>
        public string ArtistSearchTerm
        {
            get => _artistSearchTerm;
            set
            {
                RaiseAndSetIfChanged(ref _artistSearchTerm, value, nameof(ArtistSearchTerm));
                SearchArtists.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets a command that searches for setlists based on the artist search term.
        /// </summary>
        public RelayCommand<object> SearchArtists => new (
            _ => !string.IsNullOrEmpty(ArtistSearchTerm),
            async _ =>
            {
                var setlists = await _setlistSearch.SearchForSetlistsAsync(ArtistSearchTerm, CancellationToken.None).ConfigureAwait(false);
                Setlists = setlists.ToList();
            });

        /// <summary>
        /// Gets a command that selects a setlist and raises the SetlistSelected event.
        /// </summary>
        public RelayCommand<Setlist> SelectSetlist => new(setlist =>
        {
            SetlistSelected?.Invoke(this, new SetlistSelectedEventArgs(setlist));
        });

        /// <summary>
        /// Gets or sets the list of setlists for the searched artist.
        /// </summary>
        public List<Setlist> Setlists
        {
            get => _setlists;
            set => RaiseAndSetIfChanged(ref _setlists, value, nameof(Setlists));
        }
    }
}
