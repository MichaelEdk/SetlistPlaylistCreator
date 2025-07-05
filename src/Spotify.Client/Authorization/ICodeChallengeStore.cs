namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Interface for storing and retrieving code challenges used in the OAuth 2.0 authorization code flow with PKCE (Proof Key for Code Exchange).
    /// </summary>
    public interface ICodeChallengeStore
    {
        /// <summary>
        /// Retrieves the code challenge used for PKCE in the OAuth 2.0 authorization code flow.
        /// </summary>
        /// <returns>The code challenge used for PKCE in the OAuth 2.0 authorization code flow.</returns>
        string RetrieveChallenge();

        /// <summary>
        /// Stores the code challenge used for PKCE in the OAuth 2.0 authorization code flow.
        /// </summary>
        /// <param name="challenge">The code challenge.</param>
        void StoreChallenge(string challenge);
    }
}
