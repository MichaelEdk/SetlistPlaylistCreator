using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IAccessTokenHttpClient _accessTokenHttpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHttpClient"/> class.
        /// </summary>
        /// <param name="accessTokenHttpClient">Provides Spotify access tokens.</param>
        /// <param name="httpClientFactory">A IHttpClientFactory instance to use for making requests to the Spotify API.</param>
        public SearchHttpClient(IAccessTokenHttpClient accessTokenHttpClient, IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(accessTokenHttpClient, nameof(accessTokenHttpClient));
            ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            _accessTokenHttpClient = accessTokenHttpClient;
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown if there was an error with the HTTP request.</exception>
        public async Task<SearchResult> SearchSongsAsync(string songTitle, string artistName)
        {
            var token = await _accessTokenHttpClient.GetAccessTokenAsync().ConfigureAwait(false);

            using var httpClient = _httpClientFactory.CreateClient(nameof(SearchHttpClient));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var queryParams = new Dictionary<string, string?>
            {
                ["q"] = $"{songTitle} track:{songTitle} artist:{artistName}",
                ["type"] = "track"
            };

            var queryString = QueryHelpers.AddQueryString(string.Empty, queryParams);

            var response = await httpClient.GetAsync($"search{queryString}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new SpotifyApiException($"Failed to search songs. Http status code: [{response.StatusCode}].");
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<SearchResult>(json) ?? throw new SpotifyApiException($"Result object is null. Json payload: '{json}'");
        }
    }
}
