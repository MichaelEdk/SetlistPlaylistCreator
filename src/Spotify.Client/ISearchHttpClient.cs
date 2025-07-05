namespace Spotify.Client
{
    /// <summary>
    /// An HTTP client interface for searching songs on Spotify's API.
    /// </summary>
    public interface ISearchHttpClient
    {
        /// <summary>
        /// Asynchronously searches for songs on Spotify's API using the provided song title and artist name.
        /// </summary>
        /// <param name="songTitle">The title of the song to search.</param>
        /// <param name="artistName">The artist of the song's name.</param>
        /// <returns>A <see cref="SearchResult"/> containing results of the search operation.</returns>
        Task<SearchResult> SearchSongsAsync(string songTitle, string artistName);
    }
}