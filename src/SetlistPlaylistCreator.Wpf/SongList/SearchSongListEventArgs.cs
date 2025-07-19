using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.Wpf.SongList
{
    /// <summary>
    /// Event arguments for when a setlist is used to searched for songs on a streaming platform.
    /// </summary>
    public class SearchSongListEventArgs
        : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSongListEventArgs"/> class.
        /// </summary>
        /// <param name="songList">The setlist to search on a streaming platform.</param>
        public SearchSongListEventArgs(Setlist songList)
        {
            ArgumentNullException.ThrowIfNull(songList, nameof(songList));

            SongList = songList;
        }

        /// <summary>
        /// Gets the setlist to search on a streaming platform.
        /// </summary>
        public Setlist SongList { get; }
    }
}
