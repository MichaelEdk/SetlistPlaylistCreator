namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Interface for building the authorization code URL for Spotify's OAuth 2.0 authorization flow.
    /// </summary>
    public interface IAuthorizationCodeUrlBuilder
    {
        /// <summary>
        /// Builds the authorization code URL for Spotify's OAuth 2.0 authorization flow.
        /// </summary>
        /// <param name="clientId">The Spotify client id. Provided when requesting an API from Spotify.</param>
        /// <param name="redirectUri">The redirect URI configured in the Spotify admin site.</param>
        /// <returns>The built URI.</returns>
        Uri BuildUri(string clientId, string redirectUri);
    }
}