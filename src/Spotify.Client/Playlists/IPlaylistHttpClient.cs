namespace Spotify.Client.Playlists
{
    /// <summary>
    /// Makes HTTP calls related to playlists.
    /// </summary>
    public interface IPlaylistHttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playlistName"></param>
        /// <returns></returns>
        Task<string> CreatePlaylistAsync(string userId, string playlistName);

        Task AddSongsToPlaylist(string playlistId, IEnumerable<string> songUris);
    }
}