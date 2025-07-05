using Spotify.Client;

namespace SetlistPlaylistCreator.WebApi
{
    public class TokenStore : ITokenStore, IAccessTokenProvider
    {
        private string? _token;

        public Task<string> GetTokenAsync()
        {
            return Task.FromResult(_token ?? string.Empty);
        }

        public void StoreToken(string token)
        {
            _token = token;
        }
    }
}
