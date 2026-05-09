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
                ),
                new Setlist(
                    Name: "Shea Stadium, New York, NY",
                    ArtistName: "The Beatles",
                    Sets:
                    [
                        new SetlistSet(
                            Name: "Set 1",
                            Songs:
                            [
                                new SetlistSong("Twist and Shout", "The Beatles"),
                                new SetlistSong("She's a Woman", "The Beatles"),
                                new SetlistSong("I Feel Fine", "The Beatles"),
                                new SetlistSong("Dizzy Miss Lizzy", "The Beatles"),
                                new SetlistSong("Ticket to Ride", "The Beatles"),
                                new SetlistSong("Everybody's Trying to Be My Baby", "The Beatles"),
                                new SetlistSong("Can't Buy Me Love", "The Beatles"),
                                new SetlistSong("Baby's in Black", "The Beatles"),
                                new SetlistSong("Act Naturally", "The Beatles"),
                                new SetlistSong("A Hard Day's Night", "The Beatles"),
                                new SetlistSong("Help!", "The Beatles"),
                                new SetlistSong("I'm Down", "The Beatles")
                            ]
                        )
                    ]
                ),
                new Setlist(
                    Name: "Abbey Road Studio Sessions",
                    ArtistName: "The Beatles",
                    Sets:
                    [
                        new SetlistSet(
                            Name: "Studio Recording",
                            Songs:
                            [
                                new SetlistSong("Come Together", "The Beatles"),
                                new SetlistSong("Something", "The Beatles"),
                                new SetlistSong("Maxwell's Silver Hammer", "The Beatles"),
                                new SetlistSong("Oh! Darling", "The Beatles"),
                                new SetlistSong("Octopus's Garden", "The Beatles"),
                                new SetlistSong("I Want You (She's So Heavy)", "The Beatles"),
                                new SetlistSong("Here Comes the Sun", "The Beatles")
                            ]
                        )
                    ]
                ),
                new Setlist(
                    Name: "Hollywood Bowl, Los Angeles, CA",
                    ArtistName: "The Beatles",
                    Sets:
                    [
                        new SetlistSet(
                            Name: "Main Set",
                            Songs:
                            [
                                new SetlistSong("Rock and Roll Music", "Chuck Berry"),
                                new SetlistSong("She's a Woman", "The Beatles"),
                                new SetlistSong("If I Needed Someone", "The Beatles"),
                                new SetlistSong("Day Tripper", "The Beatles"),
                                new SetlistSong("Baby's in Black", "The Beatles"),
                                new SetlistSong("I Feel Fine", "The Beatles"),
                                new SetlistSong("Yesterday", "The Beatles"),
                                new SetlistSong("I Wanna Be Your Man", "The Beatles"),
                                new SetlistSong("Nowhere Man", "The Beatles"),
                                new SetlistSong("Paperback Writer", "The Beatles"),
                                new SetlistSong("Long Tall Sally", "Little Richard")
                            ]
                        )
                    ]
                ),
                new Setlist(
                    Name: "Budokan Hall, Tokyo, Japan",
                    ArtistName: "The Beatles",
                    Sets:
                    [
                        new SetlistSet(
                            Name: "Set 1",
                            Songs:
                            [
                                new SetlistSong("Rock and Roll Music", "Chuck Berry"),
                                new SetlistSong("She's a Woman", "The Beatles"),
                                new SetlistSong("If I Needed Someone", "The Beatles"),
                                new SetlistSong("Day Tripper", "The Beatles"),
                                new SetlistSong("Baby's in Black", "The Beatles")
                            ]
                        ),
                        new SetlistSet(
                            Name: "Set 2",
                            Songs:
                            [
                                new SetlistSong("I Feel Fine", "The Beatles"),
                                new SetlistSong("Yesterday", "The Beatles"),
                                new SetlistSong("I Wanna Be Your Man", "The Beatles"),
                                new SetlistSong("Nowhere Man", "The Beatles"),
                                new SetlistSong("Paperback Writer", "The Beatles"),
                                new SetlistSong("I'm Down", "The Beatles")
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
