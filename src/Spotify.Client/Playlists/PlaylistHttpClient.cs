using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Spotify.Client.Playlists
{
    /// <summary>
    /// The default implementation of <see cref="IPlaylistHttpClient"/>.
    /// </summary>
    public class PlaylistHttpClient
        : IPlaylistHttpClient
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistHttpClient"/> class.
        /// </summary>
        /// <param name="accessTokenProvider">Provides access tokens.</param>
        /// <param name="httpClientFactory">The HTTP client used to make requests to the Spotify API.</param>
        public PlaylistHttpClient(IAccessTokenProvider accessTokenProvider, IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(accessTokenProvider, nameof(accessTokenProvider));
            ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            _accessTokenProvider = accessTokenProvider;
            _httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when the HTTP request is unsuccessful.</exception>
        public async Task AddSongsToPlaylist(string playlistId, IEnumerable<string> songUris)
        {
            var token = await _accessTokenProvider.GetTokenAsync().ConfigureAwait(false);

            using var httpClient = _httpClientFactory.CreateClient(nameof(PlaylistHttpClient));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                uris = songUris
            };

            var httpResponse = await httpClient.PostAsJsonAsync($"playlists/{playlistId}/tracks", payload);

            if (!httpResponse.IsSuccessStatusCode)
            {
                // TODO: Custom exception type and better message.
                throw new Exception("Failed to add songs to playlist");
            }
        }

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown when the HTTP request is unsuccessful.</exception>
        public async Task<string> CreatePlaylistAsync(string userId, string playlistName)
        {
            var token = await _accessTokenProvider.GetTokenAsync().ConfigureAwait(false);

            using var httpClient = _httpClientFactory.CreateClient(nameof(PlaylistHttpClient));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var playlist = new CreatePlaylistRequest
            {
                Name = playlistName
            };

            var httpResponse = await httpClient.PostAsJsonAsync($"users/{userId}/playlists", playlist).ConfigureAwait(false);

            await using var json = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                // TODO: Custom exception type and better message.
                throw new Exception($"Failed to create playlist. Response {json}");
            }

            var response = JsonSerializer.Deserialize<CreatePlaylistResponse>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return response?.Id ?? string.Empty;
        }

        private class CreatePlaylistRequest
        {
            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; }

            public bool Public { get; set; }
        }

        private class CreatePlaylistResponse
        {
            public string Id { get; set; } = string.Empty;
        }
    }
}
