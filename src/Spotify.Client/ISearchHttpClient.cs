namespace Spotify.Client
{
    public interface ISearchHttpClient
    {
        Task<SearchResult> SearchSongsAsync(string songTitle, string artistName);
    }
}