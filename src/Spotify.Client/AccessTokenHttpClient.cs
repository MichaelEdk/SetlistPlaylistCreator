using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spotify.Client.Configuration;

namespace Spotify.Client
{
    /// <summary>
    /// The default implementation of <see cref="IAccessTokenHttpClient"/>.
    /// </summary>
    public class AccessTokenHttpClient
        : IAccessTokenHttpClient
    {
        private readonly SpotifyOptions _spotifyOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenHttpClient"/> class.
        /// </summary>
        /// <param name="spotifyOptions">Configuration options related to Spotify.</param>
        /// <param name="httpClientFactory">The HTTP client factory used to make requests to the Spotify API.</param>
        public AccessTokenHttpClient(IOptions<SpotifyOptions> spotifyOptions, IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(spotifyOptions, nameof(spotifyOptions));

            _spotifyOptions = spotifyOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown if the HTTP request was not successful.</exception>
        public async Task<AccessToken> GetAccessTokenAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient(nameof(AccessTokenHttpClient));

            var result = await httpClient.PostAsync("token", CreateContent()).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                throw new SpotifyApiException($"Failed to get access token. Http status code: [{result.StatusCode}].");
            }

            var json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<AccessToken>(json) ?? throw new SpotifyApiException($"Result object is null. Json payload: '{json}'");
        }

        private FormUrlEncodedContent CreateContent() => new(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _spotifyOptions.ClientId,
            ["client_secret"] = _spotifyOptions.ClientSecret
        });
    }
}
