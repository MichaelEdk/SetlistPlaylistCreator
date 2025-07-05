using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Spotify.Client
{
    public class UserHttpClient : IUserHttpClient
    {
        private readonly Uri _baseAddress = new("https://api.spotify.com/v1/");

        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IAccessTokenHttpClient _httpClient;

        public UserHttpClient(IAccessTokenProvider accessTokenProvider, IAccessTokenHttpClient httpClient)
        {
            _accessTokenProvider = accessTokenProvider;
            _httpClient = httpClient;
        }

        public async Task<User> GetUserProfileAsync()
        {
            var token = await _accessTokenProvider.GetTokenAsync().ConfigureAwait(false);

            using var client = CreateHttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("me").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: Custom exception type and better message.
                throw new Exception("Failed to get access token");
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // TODO: Custom exception type and better message.
            return JsonConvert.DeserializeObject<User>(json) ?? throw new Exception($"Result object is null. Json payload: '{json}'");
        }

        private HttpClient CreateHttpClient() => new()
        {
            BaseAddress = _baseAddress
        };
    }
}
