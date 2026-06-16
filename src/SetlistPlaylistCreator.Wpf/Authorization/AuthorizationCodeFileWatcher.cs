using System.IO;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    /// <summary>
    /// Concrete implementation of IAuthorizationCodeFileWatcher that watches the encrypted token file.
    /// Ensures the directory exists before enabling events.
    /// </summary>
    internal sealed class AuthorizationCodeFileWatcher : IAuthorizationCodeFileWatcher
    {
        private FileSystemWatcher? _watcher;

        /// <inheritdoc />
        public event FileSystemEventHandler? Changed;

        /// <inheritdoc />
        public void Enable()
        {
            // Ensure directory exists to avoid FileSystemWatcher throwing.
            if (!Directory.Exists(EncryptedTokenFile.Directory))
            {
                Directory.CreateDirectory(EncryptedTokenFile.Directory);
            }

            _watcher ??= CreateWatcher();
            _watcher.EnableRaisingEvents = true;
        }

        /// <inheritdoc />
        public void Disable()
        {
            if (_watcher is null)
            {
                return;
            }

            _watcher.EnableRaisingEvents = false;
        }

        private FileSystemWatcher CreateWatcher()
        {
            var watcher = new FileSystemWatcher(EncryptedTokenFile.Directory, EncryptedTokenFile.FileName)
            {
                NotifyFilter = NotifyFilters.LastWrite
            };

            watcher.Changed += (s, e) => Changed?.Invoke(s, e);
            return watcher;
        }
    }
}
