namespace Spotify.Client
{
    public interface IUserHttpClient
    {
        Task<User> GetUserProfileAsync();
    }
}