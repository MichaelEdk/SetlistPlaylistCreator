namespace SetlistPlaylistCreator.Domain
{
    /// <summary>
    /// Represents a setlist, including its name, artist, and sets.
    /// </summary>
    /// <param name="Name">The name of the setlist.</param>
    /// <param name="ArtistName">The name of the artist for the setlist.</param>
    /// <param name="Sets">The collection of sets in the setlist.</param>
    public record Setlist(string Name, string ArtistName, IReadOnlyCollection<SetlistSet> Sets);

    /// <summary>
    /// Represents a set within a setlist, including its name and songs.
    /// </summary>
    /// <param name="Name">The name of the set.</param>
    /// <param name="Songs">The collection of songs in the set.</param>
    public record SetlistSet(string Name, IReadOnlyCollection<SetlistSong> Songs);

    /// <summary>
    /// Represents a song within a set, including its name and artist.
    /// </summary>
    /// <param name="Name">The name of the song.</param>
    /// <param name="ArtistName">The name of the artist for the song.</param>
    public record SetlistSong(string Name, string ArtistName);
}
