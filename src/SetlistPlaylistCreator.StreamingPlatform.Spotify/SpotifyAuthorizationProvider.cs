using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client.Authorization;

namespace SetlistPlaylistCreator.StreamingPlatform.Spotify
{
    /// <summary>
    /// A Spotify-specific implementation of <see cref="IAuthorizationProvider"/>.
    /// </summary>
    public class SpotifyAuthorizationProvider : IAuthorizationProvider
    {
        private readonly IAuthorizationCodeUrlBuilder _authorizationCodeUrlBuilder;
        private readonly SpotifyOptions _spotifyOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyAuthorizationProvider"/> class.
        /// </summary>
        /// <param name="authorizationCodeUrlBuilder">Builds authorization code URLs.</param>
        /// <param name="spotifyOptions">Spotify options.</param>
        public SpotifyAuthorizationProvider(
            IAuthorizationCodeUrlBuilder authorizationCodeUrlBuilder,
            IOptions<SpotifyOptions> spotifyOptions)
        {
            _authorizationCodeUrlBuilder = authorizationCodeUrlBuilder;
            _spotifyOptions = spotifyOptions.Value;
        }

        /// <inheritdoc />
        public Uri GetAuthorizationUrl(Uri redirectAddress)
        {
            return _authorizationCodeUrlBuilder.BuildUri(_spotifyOptions.ClientId, redirectAddress.AbsoluteUri);
        }
    }
}
