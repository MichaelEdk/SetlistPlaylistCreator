namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Interface for storing and retrieving the authorization code used in the OAuth 2.0 authorization code flow.
    /// </summary>
    public interface IAuthorizationCodeStore
    {
        /// <summary>
        /// Clears the stored authorization code, effectively removing it from the store.
        /// </summary>
        void ClearStore();

        /// <summary>
        /// Retrieves the stored authorization code.
        /// </summary>
        /// <returns>The stored authorization code.</returns>
        string? RetrieveCode();

        /// <summary>
        /// Stores the authorization code for later use.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        void StoreCode(string code);
    }
}
