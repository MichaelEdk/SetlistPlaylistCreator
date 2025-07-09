using SetlistPlaylistCreator.Domain;

namespace SetlistPlaylistCreator.Service
{
    /// <summary>
    /// Represents a song that has been searched for on the streaming platform.
    /// </summary>
    /// <param name="firstMatch">First match found for the song on the streaming platform.</param>
    /// <param name="allMatches">All matches found for the song on the streaming platform.</param>
    /// <param name="searchedSongName">Song name used in the search query to find the song on the streaming platform.</param>
    /// <param name="searchedArtistName">Artist name used to find the song on the streaming platform.</param>
    public class SearchedSong(StreamingPlatformSong firstMatch, IReadOnlyCollection<StreamingPlatformSong> allMatches, string searchedSongName, string searchedArtistName)
    {
        /// <summary>
        /// Gets the first match found for the song on the streaming platform.
        /// </summary>
        public StreamingPlatformSong FirstMatch { get; } = firstMatch;

        /// <summary>
        /// Gets all matches found for the song on the streaming platform.
        /// </summary>
        public IReadOnlyCollection<StreamingPlatformSong> AllMatches { get; } = allMatches;

        /// <summary>
        /// Gets the song name used in the search query to find the song on the streaming platform.
        /// </summary>
        public string SearchedSongName { get; } = searchedSongName;

        /// <summary>
        /// Gets the artist name used to find the song on the streaming platform.
        /// </summary>
        public string SearchedArtistName { get; } = searchedArtistName;
    }
}
