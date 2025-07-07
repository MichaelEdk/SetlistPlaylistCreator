namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Defines a contract for generating a hash from a given string.
    /// </summary>
    public interface IHashGenerator
    {
        /// <summary>
        /// Generates a hash value for the specified input string.
        /// </summary>
        /// <param name="stringToHash">The input string to hash.</param>
        /// <returns>The generated hash as a string.</returns>
        string GenerateHash(string stringToHash);
    }
}