namespace Spotify.Client.Playlists
{
    /// <summary>
    /// Makes HTTP calls related to playlists.
    /// </summary>
    public interface IPlaylistHttpClient
    {
        /// <summary>
        /// Creates a new playlist for the specified user asynchronously.
        /// </summary>
        /// <param name="userId">The Spotify user's ID.</param>
        /// <param name="playlistName">The name of the playlist to create.</param>
        /// <returns>The Spotify identifier of the newly created playlist.</returns>
        Task<string> CreatePlaylistAsync(string userId, string playlistName);

        /// <summary>
        /// Adds songs to an existing playlist asynchronously.
        /// </summary>
        /// <param name="playlistId">The Spotify ID of the playlist.</param>
        /// <param name="songUris">The Spotify URIs of the songs to add to the playlist.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddSongsToPlaylist(string playlistId, IEnumerable<string> songUris);
    }
}