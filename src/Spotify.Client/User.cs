using Newtonsoft.Json;

namespace Spotify.Client
{
    public class ExplicitContent
    {
        [JsonProperty("filter_enabled")]
        public bool FilterEnabled { get; set; }

        [JsonProperty("filter_locked")]
        public bool FilterLocked { get; set; }
    }

    public class User
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("explicit_content")]
        public ExplicitContent ExplicitContent { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("followers")]
        public Followers Followers { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public List<object> Images { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
