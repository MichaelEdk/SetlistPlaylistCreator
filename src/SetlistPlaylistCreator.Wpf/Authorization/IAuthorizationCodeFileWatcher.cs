using System.IO;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    /// <summary>
    /// Abstraction over a file system watcher for the authorization code file.
    /// </summary>
    public interface IAuthorizationCodeFileWatcher
    {
        /// <summary>
        /// An event raised when the authorization code file is changed, which signals that the authorization process is complete and the authorization code can be retrieved from the file.
        /// </summary>
        event FileSystemEventHandler? Changed;

        /// <summary>
        /// Enables the file watcher to start watching for changes to the authorization code file. When a change is detected, the <see cref="Changed"/> event should be raised.
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables the current component, preventing it from performing its normal operations.
        /// </summary>
        /// <remarks>After calling this method, the component will remain inactive until it is explicitly
        /// enabled again, if supported. Calling this method on an already disabled component has no effect.</remarks>
        void Disable();
    }
}
