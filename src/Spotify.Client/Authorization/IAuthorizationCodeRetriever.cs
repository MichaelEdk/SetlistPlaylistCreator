namespace Spotify.Client.Authorization
{
    public interface IAuthorizationCodeRetriever
    {
        Task<string?> RetrieveAuthorizationCodeAsync(Uri authorizationUrl);
    }
}
