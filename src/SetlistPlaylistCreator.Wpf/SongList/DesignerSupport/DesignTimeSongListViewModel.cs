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
            Setlist = new()
            {
                ArtistName = "The Beatles",
                Sets = [
                    new SetlistSet()
                    {
                        Name = "Main Set",
                        Songs = [
                            new SetlistSong { Name = "Hey Jude", ArtistName = "The Beatles" },
                            new SetlistSong { Name = "Let It Be", ArtistName = "The Beatles" },
                            new SetlistSong { Name = "Come Together", ArtistName = "Led Zeppelin" }
                        ]
                    },
                    new ()
                    {
                        Name = "Encore",
                        Songs = [
                            new SetlistSong { Name = "Twist and Shout", ArtistName = "Isely Brothers" },
                            new SetlistSong { Name = "Yesterday", ArtistName = "The Beatles" }
                        ]
                    }],
                Name = "Live at the Rooftop Concert"
            };
        }
    }
}
