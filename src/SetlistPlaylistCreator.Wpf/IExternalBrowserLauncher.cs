namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// Launches external browsers.
    /// </summary>
    public interface IExternalBrowserLauncher
    {
        /// <summary>
        /// Launches a browser, navigating to the given URI.
        /// </summary>
        /// <param name="uri">The URI to navigate to.</param>
        void Launch(Uri uri);
    }
}
