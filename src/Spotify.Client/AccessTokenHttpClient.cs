using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SetlistPlaylistCreator.WebApi.Configuration;

namespace Spotify.Client
{
    /// <summary>
    /// The default implementation of <see cref="IAccessTokenHttpClient"/>.
    /// </summary>
    public class AccessTokenHttpClient
        : IAccessTokenHttpClient
    {
        private static readonly Uri BaseAddress = new("https://accounts.spotify.com/api/");

        private readonly SpotifyOptions _spotifyOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenHttpClient"/> class.
        /// </summary>
        /// <param name="spotifyOptions">Configuration options related to Spotify.</param>
        public AccessTokenHttpClient(IOptions<SpotifyOptions> spotifyOptions)
        {
            ArgumentNullException.ThrowIfNull(spotifyOptions, nameof(spotifyOptions));

            _spotifyOptions = spotifyOptions.Value;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown if the HTTP request was not successful.</exception>
        public async Task<AccessToken> GetAccessTokenAsync()
        {
            using var httpClient = CreateHttpClient();

            var result = await httpClient.PostAsync("token", CreateContent()).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                // TODO: Custom exception type and better message.
                throw new Exception("Failed to get access token");
            }

            var json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            // TODO: Custom exception type and better message.
            return JsonConvert.DeserializeObject<AccessToken>(json) ?? throw new Exception($"Result object is null. Json payload: '{json}'");
        }

        private FormUrlEncodedContent CreateContent() => new(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _spotifyOptions.ClientId,
            ["client_secret"] = _spotifyOptions.ClientSecret
        });

        private HttpClient CreateHttpClient() => new()
        {
            BaseAddress = BaseAddress
        };
    }
}
