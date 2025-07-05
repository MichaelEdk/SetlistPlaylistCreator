using Newtonsoft.Json;

namespace Spotify.Client
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
