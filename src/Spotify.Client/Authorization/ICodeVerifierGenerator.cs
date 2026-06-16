namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Interface for generating a code verifier for OAuth 2.0 authorization.
    /// </summary>
    public interface ICodeVerifierGenerator
    {
        /// <summary>
        /// Generates a random string to be used as a code verifier in OAuth 2.0 authorization.
        /// </summary>
        /// <returns>A random string.</returns>
        string GenerateRandomString();
    }
}