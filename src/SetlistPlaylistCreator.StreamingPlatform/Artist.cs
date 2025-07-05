using System.Text.Json.Serialization;

namespace SetlistPlaylistCreator.StreamingPlatform
{
    /// <summary>
    /// Represents a music artist.
    /// </summary>
    public record Artist
    {
        /// <summary>
        /// Gets or sets the name of the artist.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Artist"/> record.
        /// </summary>
        /// <param name="Name">The name of the artist.</param>
        [JsonConstructor]
        public Artist(string Name)
        {
            this.Name = Name;
        }
    }

    /// <summary>
    /// Represents a song with a title, artist, and unique identifier.
    /// </summary>
    public record Song
    {
        /// <summary>
        /// Gets or sets the title of the song.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the artist of the song.
        /// </summary>
        [JsonPropertyName("artist")]
        public Artist Artist { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the song.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> record.
        /// </summary>
        /// <param name="Title">The title of the song.</param>
        /// <param name="Id">The unique identifier of the song.</param>
        /// <param name="Artist">The artist of the song.</param>
        [JsonConstructor]
        public Song(string Title, string Id, Artist Artist)
        {
            this.Title = Title;
            this.Artist = Artist;
            this.Id = Id;
        }
    }

    /// <summary>
    /// Represents a music album containing songs by an artist.
    /// </summary>
    public record Album
    {
        /// <summary>
        /// Gets or sets the title of the album.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the artist of the album.
        /// </summary>
        public Artist Artist { get; set; }

        /// <summary>
        /// Gets or sets the collection of songs in the album.
        /// </summary>
        public IEnumerable<Song> Songs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Album"/> record.
        /// </summary>
        /// <param name="Title">The title of the album.</param>
        /// <param name="Artist">The artist of the album.</param>
        /// <param name="Songs">The collection of songs in the album.</param>
        [JsonConstructor]
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
        /// Gets or sets the name of the playlist.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of songs in the playlist.
        /// </summary>
        [JsonPropertyName("songs")]
        public IEnumerable<Song> Songs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> record.
        /// </summary>
        /// <param name="Name">The name of the playlist.</param>
        /// <param name="Songs">The collection of songs in the playlist.</param>
        [JsonConstructor]
        public Playlist(string Name, IEnumerable<Song> Songs)
        {
            this.Name = Name;
            this.Songs = Songs;
        }
    }
}
