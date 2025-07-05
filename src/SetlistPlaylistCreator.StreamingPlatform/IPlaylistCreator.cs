namespace SetlistPlaylistCreator.StreamingPlatform
{
    /// <summary>
    /// Handles the creation of playlists on a streaming platform.
    /// </summary>
    public interface IPlaylistCreator
    {
        /// <summary>
        /// Creates the given playlist on the streaming platform.
        /// </summary>
        /// <param name="playlist">The playlist to create.</param>
        /// <returns>A task representing the completed asynchronous operation.</returns>
        Task CreatePlaylistAsync(Playlist playlist);
    }
}
