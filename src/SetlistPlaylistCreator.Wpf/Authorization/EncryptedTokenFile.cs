using System.IO;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    internal class EncryptedTokenFile
    {
        public static string Directory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SetlistPlaylistCreator");

        public static string FileName => "Code.dat";

        public static string FilePath => Path.Combine(Directory, FileName);
    }
}
