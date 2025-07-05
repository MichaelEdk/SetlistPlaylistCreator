using System.Security.Cryptography;
using System.Text;

namespace Spotify.Client.Authorization
{
    /// <summary>
    /// An implementation of <see cref="IHashGenerator"/> that generates SHA256 hashes.
    /// </summary>
    public class Sha256HashGenerator
        : IHashGenerator
    {
        /// <inheritdoc />
        public string GenerateHash(string stringToHash)
        {
            using var sha256 = SHA256.Create();

            var inputBytes = Encoding.UTF8.GetBytes(stringToHash);
            var hashBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(hashBytes)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");
        }
    }
}
