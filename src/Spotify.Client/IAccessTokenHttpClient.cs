namespace Spotify.Client
{
    /// <summary>
    /// An HTTP client interface for retrieving access tokens from Spotify's API.
    /// </summary>
    public interface IAccessTokenHttpClient
    {
        /// <summary>
        /// Asynchronously retrieves an access token from Spotify's API.
        /// </summary>
        /// <returns>The access token DTO object received from the Spotify API.</returns>
        Task<AccessToken> GetAccessTokenAsync();
    }
}