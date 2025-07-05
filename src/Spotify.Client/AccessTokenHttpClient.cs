using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SetlistPlaylistCreator.WebApi.Configuration;

namespace Spotify.Client
{
    public class AccessTokenHttpClient : IAccessTokenHttpClient
    {
        private readonly Uri _baseAddress = new("https://accounts.spotify.com/api/");

        private readonly SpotifyOptions _spotifyOptions;

        public AccessTokenHttpClient(IOptions<SpotifyOptions> spotifyOptions)
        {
            _spotifyOptions = spotifyOptions.Value;
        }

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
            BaseAddress = _baseAddress
        };
    }
}
