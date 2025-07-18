using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.SetlistPlatform;

namespace SetlistPlaylistCreator.Wpf.ArtistSearch.DesignerSupport
{
    /// <summary>
    /// A design-time view model for the <see cref="ArtistSearchViewModel"/> class.
    /// </summary>
    internal class DesignTimeArtistSearchViewModel
        : ArtistSearchViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeArtistSearchViewModel"/> class with sample data.
        /// </summary>
        public DesignTimeArtistSearchViewModel()
            : base(NullSetlistSearch.Instance)
        {
            // Sample data for design time
            Setlists =
            [
                new Setlist(
                    Name: "Live at the Rooftop Concert",
                    ArtistName: "The Beatles",
                    Sets:
                    [
                        new SetlistSet(
                            Name: "Main Set",
                            Songs:
                            [
                                new SetlistSong("Hey Jude", "The Beatles"),
                                new SetlistSong("Let It Be", "The Beatles"),
                                new SetlistSong("Come Together", "Led Zeppelin")
                            ]
                        ),
                        new SetlistSet(
                            Name: "Encore",
                            Songs:
                            [
                                new SetlistSong("Twist and Shout", "Isley Brothers"),
                                new SetlistSong("Yesterday", "The Beatles")
                            ]
                        )
                    ]
                )
            ];
        }

        private class NullSetlistSearch
            : ISetlistSearch
        {
            public static readonly NullSetlistSearch Instance = new();

            public Task<IReadOnlyCollection<Setlist>> SearchForSetlistsAsync(string artist, CancellationToken cancellationToken) =>
                Task.FromResult<IReadOnlyCollection<Setlist>>([]);
        }
    }
}
