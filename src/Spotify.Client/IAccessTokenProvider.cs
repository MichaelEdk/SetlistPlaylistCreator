namespace Spotify.Client
{
    /// <summary>
    /// Interface for providing access tokens for Spotify API requests.
    /// </summary>
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// Asynchronously retrieves the access token for Spotify API requests.
        /// </summary>
        /// <returns>A string representation of the token to use in future requests.</returns>
        Task<string> GetTokenAsync();
    }
}