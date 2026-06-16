using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Spotify.Client
{
    /// <summary>
    /// Default implementation of <see cref="IUserHttpClient"/>.
    /// </summary>
    public class UserHttpClient
        : IUserHttpClient
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHttpClient"/> class.
        /// </summary>
        /// <param name="accessTokenProvider">Provides access tokens.</param>
        /// <param name="httpClientFactory">The HTTP client used to make requests to the Spotify API.</param>
        public UserHttpClient(IAccessTokenProvider accessTokenProvider, IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(accessTokenProvider, nameof(accessTokenProvider));
            ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            _accessTokenProvider = accessTokenProvider;
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc/>
        /// <exception cref="SpotifyApiException">Thrown when the HTTP request is unsuccessful.</exception>
        public async Task<User> GetUserProfileAsync()
        {
            var token = await _accessTokenProvider.GetTokenAsync().ConfigureAwait(false);

            using var httpClient = _httpClientFactory.CreateClient(nameof(UserHttpClient));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("me").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new SpotifyApiException($"Failed to get user profile. Http status code: [{response.StatusCode}]. Json payload: '{responseContent}'");
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<User>(json) ?? throw new SpotifyApiException($"Result object is null. Json payload: '{json}'");
        }
    }
}
