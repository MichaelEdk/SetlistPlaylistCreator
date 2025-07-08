using Spotify.Client.Authorization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    /// <summary>
    /// An implementation of <see cref="IAuthorizationCodeStore"/> that uses DPAPI to securely store the authorization code.
    /// </summary>
    public class DpapiAuthorizationCodeStore
        : IAuthorizationCodeStore
    {
        private const string FileName = @"Code.dat";
        private readonly byte[] _entropy = [16, 234, 9, 115, 243, 204, 68, 134, 51, 39, 22, 144, 03, 168, 97, 142];
        private static string CodeFileDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SetlistPlaylistCreator");

        private static string CodeFilePath => Path.Combine(CodeFileDirectory, FileName);

        /// <inheritdoc />
        public string? RetrieveCode()
        {
            if (!File.Exists(EncryptedTokenFile.FilePath))
            {
                return null;
            }

            var encryptedData = File.ReadAllBytes(EncryptedTokenFile.FilePath);
            var decryptedData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }

        /// <inheritdoc />
        public void StoreCode(string code)
        {
            var codeBytes = Encoding.UTF8.GetBytes(code);
            var encryptedData = ProtectedData.Protect(codeBytes, _entropy, DataProtectionScope.CurrentUser);

            if (!Directory.Exists(EncryptedTokenFile.Directory))
            {
                Directory.CreateDirectory(EncryptedTokenFile.Directory);
            }

            File.WriteAllBytes(EncryptedTokenFile.FilePath, encryptedData);
        }
    }
}
