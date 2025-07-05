using Spotify.Client;

namespace SetlistPlaylistCreator.StreamingPlatform.Spotify
{
    /// <summary>
    /// A Spotify-specific implementation of <see cref="IPlatformSearch"/>.
    /// </summary>
    public class SpotifyPlatformSearch
        : IPlatformSearch
    {
        private readonly ISearchHttpClient _searchHttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyPlatformSearch"/> class.
        /// </summary>
        /// <param name="searchHttpClient">An HTTP client used to search Spotify.</param>
        public SpotifyPlatformSearch(ISearchHttpClient searchHttpClient)
        {
            _searchHttpClient = searchHttpClient;
        }

        /// <inheritdoc />
        public Task<Song> GetBestSongMatchAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Song>> SearchSongsAsync(string songTitle, string artistName)
        {
            var searchResult = await _searchHttpClient.SearchSongsAsync(songTitle, artistName).ConfigureAwait(false);

            var mapper = new SearchResultToSongsMapper();

            return mapper.Map(searchResult);
        }
    }
}