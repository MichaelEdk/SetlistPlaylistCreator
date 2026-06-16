using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.Wpf.SongList.DesignerSupport
{
    /// <summary>
    /// A design-time view model for the <see cref="SongListViewModel"/> class.
    /// </summary>
    internal class DesignTimeSongListViewModel
        : SongListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeSongListViewModel"/> class with sample data.
        /// </summary>
        public DesignTimeSongListViewModel()
            : base()
        {
            Setlist = new Setlist(
                Name: "Live at the Rooftop Concert",
                ArtistName: "The Beatles",
                Sets: [
                    new SetlistSet(
                        Name: "Main Set",
                        Songs: [
                            new SetlistSong("Hey Jude", "The Beatles"),
                            new SetlistSong("Let It Be", "The Beatles"),
                            new SetlistSong("Come Together", "Led Zeppelin")
                        ]
                    ),
                    new SetlistSet(
                        Name: "Encore",
                        Songs: [
                            new SetlistSong("Twist and Shout", "Isely Brothers"),
                            new SetlistSong("Yesterday", "The Beatles")
                        ]
                    )
                ]
            );
        }
    }
}
