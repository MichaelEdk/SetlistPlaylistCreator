namespace SetlistFm.Client
{
    /// <summary>
    /// Defines the contract for making calls to the SetlistFm API.
    /// </summary>
    public interface ISetlistSearchClient
    {
        /// <summary>
        /// Searches for setlists matching the provided artist name.
        /// </summary>
        /// <param name="artistName">The artist name to search.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="SetlistFmSetlistSearchResult"/> containing recent setlists for the given artist.</returns>
        Task<SetlistFmSetlistSearchResult> SearchSetlistsAsync(string artistName, CancellationToken cancellationToken);
    }
}