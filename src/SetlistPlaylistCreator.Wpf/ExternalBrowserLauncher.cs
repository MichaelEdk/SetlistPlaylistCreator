using System.Diagnostics;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// The default implementation of <see cref="IExternalBrowserLauncher"/>.
    /// </summary>
    internal class ExternalBrowserLauncher
        : IExternalBrowserLauncher
    {
        /// <inheritdoc />
        public void Launch(Uri uri)
        {
            // Calling Process.Start with a URL launches the default browser.
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri)
            {
                UseShellExecute = true
            });
        }
    }
}
