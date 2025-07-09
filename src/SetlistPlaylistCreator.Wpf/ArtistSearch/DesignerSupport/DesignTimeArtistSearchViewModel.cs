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
                new Setlist
                {
                    ArtistName = "The Beatles",
                    Sets =
                    [
                        new SetlistSet
                        {
                            Name = "Main Set",
                            Songs =
                            [
                                new SetlistSong { Name = "Hey Jude", ArtistName = "The Beatles" },
                                new SetlistSong { Name = "Let It Be", ArtistName = "The Beatles" },
                                new SetlistSong { Name = "Come Together", ArtistName = "Led Zeppelin" }
                            ]
                        },
                        new SetlistSet
                        {
                            Name = "Encore",
                            Songs =
                            [
                                new SetlistSong { Name = "Twist and Shout", ArtistName = "Isley Brothers" },
                                new SetlistSong { Name = "Yesterday", ArtistName = "The Beatles" }
                            ]
                        }
                    ],
                    Name = "Live at the Rooftop Concert"
                }
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
