using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.SetlistPlatform
{
    /// <summary>
    /// Contract for searching for setlists.
    /// </summary>
    public interface ISetlistSearch
    {
        /// <summary>
        /// Asynchronously searches for setlists by artist name.
        /// </summary>
        /// <param name="artist">The artist to search for.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A collection of setlists for the given artist.</returns>
        Task<IReadOnlyCollection<Setlist>> SearchForSetlistsAsync(string artist, CancellationToken cancellationToken);
    }
}
