using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Spotify.Client
{
    public class UserHttpClient
        : IUserHttpClient
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserHttpClient(IAccessTokenProvider accessTokenProvider, IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(accessTokenProvider, nameof(accessTokenProvider));
            ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            _accessTokenProvider = accessTokenProvider;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User> GetUserProfileAsync()
        {
            var token = await _accessTokenProvider.GetTokenAsync().ConfigureAwait(false);

            using var httpClient = _httpClientFactory.CreateClient(nameof(UserHttpClient));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("me").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyApiException($"Failed to get user profile. Http status code: [{response.StatusCode}].");
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<User>(json) ?? throw new SpotifyApiException($"Result object is null. Json payload: '{json}'");
        }
    }
}
