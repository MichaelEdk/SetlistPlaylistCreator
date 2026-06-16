using System.Security.Cryptography;
using System.Text;

namespace Spotify.Client.Authorization
{
    /// <summary>
    /// Default implementation of <see cref="ICodeVerifierGenerator"/>.
    /// </summary>
    public class CodeVerifierGenerator
        : ICodeVerifierGenerator
    {
        /// <inheritdoc/>
        public string GenerateRandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";
            byte[] randomBytes = new byte[64];

            RandomNumberGenerator.Fill(randomBytes);

            var stringBuilder = new StringBuilder();

            foreach (byte b in randomBytes)
            {
                stringBuilder.Append(chars[b % chars.Length]);
            }

            return stringBuilder.ToString();
        }
    }
}
