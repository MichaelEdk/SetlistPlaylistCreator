namespace SetlistPlaylistCreator.Domain
{
    /// <summary>
    /// Represents a setlist, including its name, artist, and sets.
    /// </summary>
    public record Setlist
    {
        /// <summary>
        /// Gets or sets the name of the setlist.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the artist for the setlist.
        /// </summary>
        public string ArtistName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of sets in the setlist.
        /// </summary>
        public IReadOnlyCollection<SetlistSet> Sets { get; set; } = [];
    }

    /// <summary>
    /// Represents a set within a setlist, including its name and songs.
    /// </summary>
    public record SetlistSet
    {
        /// <summary>
        /// Gets or sets the name of the set.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of songs in the set.
        /// </summary>
        public IReadOnlyCollection<SetlistSong> Songs { get; set; } = [];
    }

    /// <summary>
    /// Represents a song within a set, including its name and artist.
    /// </summary>
    public record SetlistSong
    {
        /// <summary>
        /// Gets or sets the name of the song.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the artist for the song.
        /// </summary>
        public string ArtistName { get; set; } = string.Empty;
    }
}
