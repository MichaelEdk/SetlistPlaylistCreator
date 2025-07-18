namespace SetlistPlaylistCreator.Domain
{
    /// <summary>
    /// Represents a song on a streaming platform, identified by its name, artist, and unique ID.
    /// </summary>
    /// <param name="SongName">The name of the song.</param>
    /// <param name="ArtistNames">The names of the song's artists.</param>
    /// <param name="Id">The streaming platform's unique identifier for the song.</param>
    public record StreamingPlatformSong(string SongName, IReadOnlyCollection<string> ArtistNames, string Id);
}
