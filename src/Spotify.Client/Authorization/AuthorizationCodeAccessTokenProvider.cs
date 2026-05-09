using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;

namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Provides access tokens for Spotify API requests using the OAuth 2.0 authorization code flow with PKCE (Proof Key for Code Exchange).
    /// </summary>
    public class AuthorizationCodeAccessTokenProvider : IAccessTokenProvider
    {
        private const string TokenEndpoint = "https://accounts.spotify.com/api/token";

        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly ICodeChallengeStore _codeChallengeStore;
        private readonly SpotifyOptions _spotifyOptions;

        private string _accessToken = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeAccessTokenProvider"/> class.
        /// </summary>
        /// <param name="authorizationCodeStore">The store for retrieving and storing the authorization code.</param>
        /// <param name="codeChallengeStore">The store for retrieving and storing the code challenge used for PKCE.</param>
        /// <param name="spotifyOptions">The options monitor for accessing Spotify configuration values.</param>
        public AuthorizationCodeAccessTokenProvider(
            IAuthorizationCodeStore authorizationCodeStore,
            ICodeChallengeStore codeChallengeStore,
            IOptionsMonitor<SpotifyOptions> spotifyOptions)
        {
            _authorizationCodeStore = authorizationCodeStore;
            _codeChallengeStore = codeChallengeStore;
            _spotifyOptions = spotifyOptions.CurrentValue;
        }

        /// <inheritdoc />
        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var code = _authorizationCodeStore.RetrieveCode();

            if (code == null)
            {
                return string.Empty;
            }

            var codeVerifier = _codeChallengeStore.RetrieveChallenge();

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", _spotifyOptions.RedirectAddress),
                    new KeyValuePair<string, string>("client_id", _spotifyOptions.ClientId),
                    new KeyValuePair<string, string>("code_verifier", codeVerifier),
                });

                HttpResponseMessage response = await client.PostAsync(TokenEndpoint, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Once the authorization code has been used to generate a token successfully, it can't be used again.
                    // Clear it from the store to prevent reuse.
                    _authorizationCodeStore.ClearStore();

                    // Parse the response JSON to get the access token and refresh token
                    // Assumes the response contains "access_token" and "refresh_token" fields
                    // You may need to adjust this parsing logic based on the actual response format
                    dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

                    if (jsonResponse == null)
                    {
                        throw new InvalidOperationException($"Failed to deserialize the response from Spotify API. Raw response content: {responseContent}");
                    }

                    _accessToken = jsonResponse.access_token;

                    return _accessToken;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
