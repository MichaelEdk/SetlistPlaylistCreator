namespace SetlistPlaylistCreator.WebApi.Configuration
{
    /// <summary>
    /// Configuration options for Spotify.
    /// </summary>
    public sealed class SpotifyOptions
    {
        public const string Name = "Spotify";

        /// <summary>
        /// Gets or sets the client id issued by Spotify.
        /// </summary>
        public string ClientId { get; set; } = "";

        /// <summary>
        /// Gets or sets the client secret issued by Spotify.
        /// </summary>
        public string ClientSecret { get; set; } = "";

        /// <summary>
        /// Gets or sets the address that Spotify should redirect to after authorization.
        /// </summary>
        public string RedirectAddress { get; set; } = "";
    }
}
