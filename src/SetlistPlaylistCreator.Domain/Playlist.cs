namespace SetlistPlaylistCreator.Domain
{
    /// <summary>
    /// Represents the artist of a song in a playlist.
    /// </summary>
    /// <param name="Name">The name of the artist.</param>
    public record PlaylistArtist(string Name);

    /// <summary>
    /// Represents a song in a playlist.
    /// </summary>
    /// <param name="Title">The song's title.</param>
    /// <param name="Id">A unique identifier of the song in the streaming platform.</param>
    /// <param name="Artists">The song's artists.</param>
    public record PlaylistSong(string Title, string Id, IReadOnlyCollection<PlaylistArtist> Artists);

    /// <summary>
    /// Represents an album in a playlist, containing multiple songs.
    /// </summary>
    /// <param name="Title">The album's title.</param>
    /// <param name="Artists">The album's artists.</param>
    /// <param name="Songs">The songs in the album.</param>
    public record PlaylistAlbum(string Title, IReadOnlyCollection<PlaylistArtist> Artists, IReadOnlyCollection<PlaylistSong> Songs);

    /// <summary>
    /// Represents a playlist containing multiple songs and albums.
    /// </summary>
    /// <param name="Name">The name of the playlist.</param>
    /// <param name="Songs">The songs in the playlist.</param>
    public record Playlist(string Name, IEnumerable<PlaylistSong> Songs);
}