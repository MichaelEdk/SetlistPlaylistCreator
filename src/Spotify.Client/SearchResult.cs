using Newtonsoft.Json;

namespace Spotify.Client
{
    /// <summary>
    /// Represents a music album with detailed metadata from Spotify.
    /// </summary>
    public class Album
    {
        /// <summary>
        /// Gets or sets the type of the album (e.g., album, single, compilation) - AI generated. TODO: check..
        /// </summary>
        [JsonProperty("album_type")]
        public string AlbumType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of tracks in the album.
        /// </summary>
        [JsonProperty("total_tracks")]
        public int TotalTracks { get; set; }

        /// <summary>
        /// Gets or sets the list of country codes where the album is available.
        /// </summary>
        [JsonProperty("available_markets")]
        public List<string> AvailableMarkets { get; set; } = [];

        /// <summary>
        /// Gets or sets the external URLs for the album.
        /// </summary>
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; } = new();

        /// <summary>
        /// Gets or sets the Spotify API endpoint for the album.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify ID for the album.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the images of the album in various sizes.
        /// </summary>
        [JsonProperty("images")]
        public List<Image> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the name of the album.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the release date of the album.
        /// </summary>
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the precision of the release date (year, month, or day).
        /// </summary>
        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the restrictions on the album (if any).
        /// </summary>
        [JsonProperty("restrictions")]
        public Restrictions Restrictions { get; set; } = new();

        /// <summary>
        /// Gets or sets the object type (should be "album").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify URI for the album.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the copyright statements for the album.
        /// </summary>
        [JsonProperty("copyrights")]
        public List<Copyright> Copyrights { get; set; } = [];

        /// <summary>
        /// Gets or sets the external IDs (ISRC, EAN, UPC) for the album.
        /// </summary>
        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the genres associated with the album.
        /// </summary>
        [JsonProperty("genres")]
        public List<string> Genres { get; set; } = [];

        /// <summary>
        /// Gets or sets the label for the album.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the popularity of the album (0-100).
        /// </summary>
        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        /// <summary>
        /// Gets or sets the group of the album (e.g., "album", "single", "compilation", "appears_on").
        /// </summary>
        [JsonProperty("album_group")]
        public string AlbumGroup { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of artists for the album.
        /// </summary>
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; } = [];
    }

    /// <summary>
    /// Represents a music artist with detailed metadata from Spotify.
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// Gets or sets the external URLs for the artist.
        /// </summary>
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; } = new();

        /// <summary>
        /// Gets or sets the Spotify API endpoint for the artist.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify ID for the artist.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the artist.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the object type (should be "artist") TODO: AI generated - check.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify URI for the artist.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the followers information for the artist.
        /// </summary>
        [JsonProperty("followers")]
        public Followers Followers { get; set; } = new();

        /// <summary>
        /// Gets or sets the genres associated with the artist.
        /// </summary>
        [JsonProperty("genres")]
        public List<string> Genres { get; set; } = [];

        /// <summary>
        /// Gets or sets the images of the artist in various sizes.
        /// </summary>
        [JsonProperty("images")]
        public List<Image> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the popularity of the artist (0-100).
        /// </summary>
        [JsonProperty("popularity")]
        public int Popularity { get; set; }
    }

    /// <summary>
    /// Represents a copyright statement for an album.
    /// </summary>
    public class Copyright
    {
        /// <summary>
        /// Gets or sets the copyright text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of copyright (e.g., "C" or "P").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents external IDs for an album or track (ISRC, EAN, UPC).
    /// </summary>
    public class ExternalIds
    {
        /// <summary>
        /// Gets or sets the International Standard Recording Code.
        /// </summary>
        [JsonProperty("isrc")]
        public string Isrc { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the International Article Number.
        /// </summary>
        [JsonProperty("ean")]
        public string Ean { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Universal Product Code.
        /// </summary>
        [JsonProperty("upc")]
        public string Upc { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents external URLs for a Spotify object.
    /// </summary>
    public class ExternalUrls
    {
        /// <summary>
        /// Gets or sets the Spotify URL.
        /// </summary>
        [JsonProperty("spotify")]
        public string Spotify { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the followers information for an artist.
    /// </summary>
    public class Followers
    {
        /// <summary>
        /// Gets or sets the Spotify API endpoint for the followers.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of followers.
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }
    }

    /// <summary>
    /// Represents an image object with URL and dimensions.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets the source URL of the image.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image height in pixels.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the image width in pixels.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }
    }

    /// <summary>
    /// Represents a track item in a search result.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the album the track belongs to.
        /// </summary>
        [JsonProperty("album")]
        public Album Album { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of artists for the track.
        /// </summary>
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of country codes where the track is available.
        /// </summary>
        [JsonProperty("available_markets")]
        public List<string> AvailableMarkets { get; set; } = [];

        /// <summary>
        /// Gets or sets the disc number (usually 1 unless the album has multiple discs).
        /// </summary>
        [JsonProperty("disc_number")]
        public int DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the duration of the track in milliseconds.
        /// </summary>
        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the track has explicit lyrics.
        /// </summary>
        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        /// <summary>
        /// Gets or sets the external IDs for the track.
        /// </summary>
        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the external URLs for the track.
        /// </summary>
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; } = new();

        /// <summary>
        /// Gets or sets the Spotify API endpoint for the track.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify ID for the track.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the track is playable.
        /// </summary>
        [JsonProperty("is_playable")]
        public bool IsPlayable { get; set; }

        /// <summary>
        /// Gets or sets the linked-from information for the track (if applicable).
        /// </summary>
        [JsonProperty("linked_from")]
        public LinkedFrom LinkedFrom { get; set; } = new();

        /// <summary>
        /// Gets or sets the restrictions on the track (if any).
        /// </summary>
        [JsonProperty("restrictions")]
        public Restrictions Restrictions { get; set; } = new();

        /// <summary>
        /// Gets or sets the name of the track.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the popularity of the track (0-100).
        /// </summary>
        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        /// <summary>
        /// Gets or sets the preview URL for the track (if available).
        /// </summary>
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the track number on the album.
        /// </summary>
        [JsonProperty("track_number")]
        public int TrackNumber { get; set; }

        /// <summary>
        /// Gets or sets the object type (should be "track").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify URI for the track.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the track is a local file.
        /// </summary>
        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }
    }

    /// <summary>
    /// Represents a linked-from object for a track (currently empty).
    /// </summary>
    public class LinkedFrom
    {
    }

    /// <summary>
    /// Represents restrictions on an album or track.
    /// </summary>
    public class Restrictions
    {
        /// <summary>
        /// Gets or sets the reason for the restriction (e.g., "market", "product", "explicit").
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the result of a search query, containing tracks.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the tracks returned by the search.
        /// </summary>
        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; } = new();
    }

    /// <summary>
    /// Represents a paginated list of tracks from a search result.
    /// </summary>
    public class Tracks
    {
        /// <summary>
        /// Gets or sets the Spotify API endpoint for the tracks.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum number of items in the response (page size).
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the URL to the next page of items (if available).
        /// </summary>
        [JsonProperty("next")]
        public string Next { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the offset of the items returned (index of the first item).
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the URL to the previous page of items (if available).
        /// </summary>
        [JsonProperty("previous")]
        public string Previous { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of items available.
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the list of track items.
        /// </summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; } = [];
    }
}
