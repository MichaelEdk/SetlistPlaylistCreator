namespace Spotify.Client
{
    /// <summary>
    /// An HTTP client interface for retrieving user profile information from Spotify's API.
    /// </summary>
    public interface IUserHttpClient
    {
        /// <summary>
        /// Retrieves the user's profile information from Spotify's API asynchronously.
        /// </summary>
        /// <returns>A <see cref="User"/> representing information about the current user, based on the token provided.</returns>
        Task<User> GetUserProfileAsync();
    }
}