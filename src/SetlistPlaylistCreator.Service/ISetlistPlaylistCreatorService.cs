using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.Service
{
    /// <summary>
    /// Contract for creating a playlist on the streaming platform, based on a setlist.
    /// </summary>
    public interface ISetlistPlaylistCreatorService
    {
        /// <summary>
        /// Generates a proposed playlist based on the given setlist. This does not create a playlist in the streaming platform.
        /// </summary>
        /// <param name="setlist">The setlist.</param>
        /// <returns>A collection of songs known to the streaming platform.</returns>
        Task<IReadOnlyCollection<StreamingPlatformSong>> CreateProposedPlaylistAsync(Setlist setlist);

        /// <summary>
        /// Creates a playlist in the streaming platform with the given songs.
        /// </summary>
        /// <param name="playlistName">The name of the playlist to create.</param>
        /// <param name="songs">The songs to add to the playlist.</param>
        /// <returns><see langword="true"/> if the operation succeeded; otherwise <see langword="false"/>.</returns>
        Task<bool> CreatePlaylistAsync(string playlistName, IReadOnlyCollection<StreamingPlatformSong> songs);

    }
}