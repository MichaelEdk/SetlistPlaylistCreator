namespace SetlistPlaylistCreator.StreamingPlatform
{
    /// <summary>
    /// Represents a music artist.
    /// </summary>
    public record Artist
    {
        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Artist"/> record.
        /// </summary>
        /// <param name="Name">The name of the artist.</param>
        public Artist(string Name) => this.Name = Name;
    }

    /// <summary>
    /// Represents a song with a title, artist, and unique identifier.
    /// </summary>
    public record Song
    {
        /// <summary>
        /// Gets the title of the song.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the artists of the song.
        /// </summary>
        public IReadOnlyCollection<Artist> Artists { get; }

        /// <summary>
        /// Gets the unique identifier of the song.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> record.
        /// </summary>
        /// <param name="Title">The title of the song.</param>
        /// <param name="Id">The unique identifier of the song.</param>
        /// <param name="Artists">The artists of the song.</param>
        public Song(string Title, string Id, IReadOnlyCollection<Artist> Artists)
        {
            this.Title = Title;
            this.Artists = Artists;
            this.Id = Id;
        }
    }

    /// <summary>
    /// Represents a music album containing songs by an artist.
    /// </summary>
    public record Album
    {
        /// <summary>
        /// Gets the title of the album.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the artist of the album.
        /// </summary>
        public Artist Artist { get; }

        /// <summary>
        /// Gets the collection of songs in the album.
        /// </summary>
        public IEnumerable<Song> Songs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Album"/> record.
        /// </summary>
        /// <param name="Title">The title of the album.</param>
        /// <param name="Artist">The artist of the album.</param>
        /// <param name="Songs">The collection of songs in the album.</param>
        public Album(string Title, Artist Artist, IEnumerable<Song> Songs)
        {
            this.Title = Title;
            this.Artist = Artist;
            this.Songs = Songs;
        }
    }

    /// <summary>
    /// Represents a playlist containing a collection of songs.
    /// </summary>
    public record Playlist
    {
        /// <summary>
        /// Gets the name of the playlist.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the collection of songs in the playlist.
        /// </summary>
        public IEnumerable<Song> Songs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> record.
        /// </summary>
        /// <param name="Name">The name of the playlist.</param>
        /// <param name="Songs">The collection of songs in the playlist.</param>
        public Playlist(string Name, IEnumerable<Song> Songs)
        {
            this.Name = Name;
            this.Songs = Songs;
        }
    }
}
