namespace SetlistPlaylistCreator.StreamingPlatform
{
    // TODO: Is this used?

    /// <summary>
    /// Contract for authorization operations for the underlying streaming platform.
    /// </summary>
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// Gets a URI the user can navigate to to perform authorization in the underlying streaming platform.
        /// </summary>
        /// <param name="redirectAddress">The redirect address. Typically used to redirect users to a page after authorization is complete.</param>
        /// <returns>An authorization URI.</returns>
        Uri GetAuthorizationUrl(Uri redirectAddress);
    }
}
