using Spotify.Client.Authorization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    public class DpapiAuthorizationCodeStore : IAuthorizationCodeStore
    {
        private const string FileName = @"Code.dat";
        private readonly byte[] _entropy = new byte[] { 16, 234, 9, 115, 243, 204, 68, 134, 51, 39, 22, 144, 03, 168, 97, 142 };

        public string? RetrieveCode()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }

            var encryptedData = File.ReadAllBytes(FileName);
            var decryptedData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }

        public void StoreCode(string code)
        {
            var codeBytes = Encoding.UTF8.GetBytes(code);
            var encryptedData = ProtectedData.Protect(codeBytes, _entropy, DataProtectionScope.CurrentUser);

            File.WriteAllBytes(FileName, encryptedData);
        }
    }
}
