namespace Spotify.Client.Authorization
{

    /// <summary>
    /// Defines a contract for retrieving an authorization code from a specified authorization URL.
    /// </summary>
    public interface IAuthorizationCodeRetriever
    {
        /// <summary>
        /// Asynchronously retrieves the authorization code from the given authorization URL.
        /// </summary>
        /// <param name="authorizationUrl">The URL to initiate the authorization process.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the authorization code as a string,
        /// or <c>null</c> if the code could not be retrieved.
        /// </returns>
        Task<string?> RetrieveAuthorizationCodeAsync(Uri authorizationUrl);
    }
}
