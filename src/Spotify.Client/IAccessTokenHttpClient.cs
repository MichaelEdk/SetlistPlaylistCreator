namespace Spotify.Client
{
    public interface IAccessTokenHttpClient
    {
        Task<AccessToken> GetAccessTokenAsync();
    }
}