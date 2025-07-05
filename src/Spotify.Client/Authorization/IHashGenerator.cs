namespace Spotify.Client.Authorization
{
    public interface IHashGenerator
    {
        string GenerateHash(string stringToHash);
    }
}