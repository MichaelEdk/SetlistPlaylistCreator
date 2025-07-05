namespace SetlistPlaylistCreator.StreamingPlatform
{
    /// <summary>
    /// Contract for searching a streaming platform.
    /// </summary>
    public interface IPlatformSearch
    {
        /// <summary>
        /// Searches for songs matching the given artist name and song title.
        /// </summary>
        /// <param name="songTitle">The title of the song to serch.</param>
        /// <param name="artistName">The artist's name.</param>
        /// <returns>A collection of potential matching songs.</returns>
        Task<IEnumerable<Song>> SearchSongsAsync(string songTitle, string artistName);

        /// <summary>
        /// Searches for a song for the given search term and returns the single best match.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The song the best matches the search term.</returns>
        Task<Song> GetBestSongMatchAsync(string searchTerm);
    }
}
