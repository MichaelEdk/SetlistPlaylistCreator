using Microsoft.Extensions.Options;
using SetlistFm.Client.Configuration;
using System.Text.Json;
using System.Web;

namespace SetlistFm.Client
{
    /// <summary>
    /// The default implementation of <see cref="ISetlistSearchClient"/>.
    /// </summary>
    public class SetlistSearchClient
        : ISetlistSearchClient
    {
        private readonly Uri _baseUri = new("https://api.setlist.fm/rest/1.0/search/setlists/");
        private static readonly JsonSerializerOptions s_camelCaseJsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private readonly SetlistFmOptions _setlistFmOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetlistSearchClient"/> class.
        /// </summary>
        /// <param name="setlistFmOptions">Configuration options for SetlistFm.</param>
        public SetlistSearchClient(
            IOptionsMonitor<SetlistFmOptions> setlistFmOptions)
        {
            ArgumentNullException.ThrowIfNull(setlistFmOptions, nameof(setlistFmOptions));
            
            _setlistFmOptions = setlistFmOptions.CurrentValue;
        }

        /// <inheritdoc />
        public async Task<SetlistFmSetlistSearchResult> SearchSetlistsAsync(string artistName, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(artistName, nameof(artistName));

            using var httpClient = new HttpClient();

            httpClient.BaseAddress = _baseUri;
            httpClient.DefaultRequestHeaders.Add("x-api-key", _setlistFmOptions.ApiKey);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await httpClient.GetAsync($"?artistName={HttpUtility.UrlEncode(artistName)}&p=1", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var rawResponseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var result = JsonSerializer.Deserialize<SetlistFmSetlistSearchResult>(rawResponseContent, s_camelCaseJsonSerializerOptions);

            if (result is null)
            {
                throw new InvalidOperationException($"Failed to deserialize the response from Setlist.fm API. Raw response content: {rawResponseContent}");
            }

            return result;
        }
    }
}