using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Spotify.Client
{
    /// <summary>
    /// The default implementation of <see cref="ISearchHttpClient"/>.
    /// </summary>
    public class SearchHttpClient
        : ISearchHttpClient
    {
        private readonly Uri _baseAddress = new("https://api.spotify.com/v1/");

        private readonly IAccessTokenHttpClient _accessTokenHttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHttpClient"/> class.
        /// </summary>
        /// <param name="accessTokenHttpClient">Provides Spotify access tokens.</param>
        public SearchHttpClient(IAccessTokenHttpClient accessTokenHttpClient)
        {
            ArgumentNullException.ThrowIfNull(accessTokenHttpClient, nameof(accessTokenHttpClient));

            _accessTokenHttpClient = accessTokenHttpClient;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown if there was an error with the HTTP request.</exception>
        public async Task<SearchResult> SearchSongsAsync(string songTitle, string artistName)
        {
            var token = await _accessTokenHttpClient.GetAccessTokenAsync().ConfigureAwait(false);

            using var client = CreateHttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var response = await client.GetAsync($"search?q={songTitle}%20track:{songTitle}%20artist:{artistName}&type=track").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: Custom exception type and better message.
                throw new Exception("Failed to get access token");
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // TODO: Custom exception type and better message.
            return JsonConvert.DeserializeObject<SearchResult>(json) ?? throw new Exception($"Result object is null. Json payload: '{json}'");
        }

        private HttpClient CreateHttpClient() => new()
        {
            BaseAddress = _baseAddress
        };
    }
}
