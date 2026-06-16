using Newtonsoft.Json;

namespace Spotify.Client
{
    /// <summary>
    /// Represents the explicit content settings for a Spotify user.
    /// </summary>
    /// <remarks>Comments are AI generated and should be verified.</remarks>
    public class ExplicitContent
    {
        /// <summary>
        /// Gets or sets a value indicating whether explicit content filtering is enabled for the user.
        /// </summary>
        [JsonProperty("filter_enabled")]
        public bool FilterEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the explicit content filter is locked and cannot be changed by the user.
        /// </summary>
        [JsonProperty("filter_locked")]
        public bool FilterLocked { get; set; }
    }

    /// <summary>
    /// Represents a Spotify user with profile and account information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the country of the user, as a ISO 3166-1 alpha-2 country code.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the explicit content settings for the user.
        /// </summary>
        [JsonProperty("explicit_content")]
        public ExplicitContent ExplicitContent { get; set; } = new();

        /// <summary>
        /// Gets or sets the external URLs for the user (e.g., Spotify profile link).
        /// </summary>
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; } = new();

        /// <summary>
        /// Gets or sets the followers information for the user.
        /// </summary>
        [JsonProperty("followers")]
        public Followers Followers { get; set; } = new();

        /// <summary>
        /// Gets or sets the Spotify API endpoint for the user.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify user ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the images of the user in various sizes.
        /// </summary>
        [JsonProperty("images")]
        public List<object> Images { get; set; } = [];

        /// <summary>
        /// Gets or sets the user's Spotify subscription product (e.g., "premium", "free").
        /// </summary>
        [JsonProperty("product")]
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the object type (should be "user").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Spotify URI for the user.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; } = string.Empty;
    }
}
