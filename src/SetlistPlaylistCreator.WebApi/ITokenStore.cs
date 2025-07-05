namespace SetlistPlaylistCreator.WebApi
{
    public interface ITokenStore
    {
        void StoreToken(string token);
    }
}
