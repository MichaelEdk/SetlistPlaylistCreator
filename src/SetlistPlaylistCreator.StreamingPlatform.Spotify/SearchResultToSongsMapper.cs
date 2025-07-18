using Spotify.Client;
using SpotifyArtist = Spotify.Client.Artist;

namespace SetlistPlaylistCreator.StreamingPlatform.Spotify
{
    // TODO: consider adding an interface or a generic mapping layer.

    /// <summary>
    /// Maps the Spotify search result to the generic streaming platform models.
    /// </summary>
    internal class SearchResultToSongsMapper
    {
        /// <summary>
        /// Maps the Spotify search result to a collection of songs.
        /// </summary>
        /// <param name="searchResult">The Spotify search result.</param>
        /// <returns>A collection of <see cref="Song"/> objects.</returns>
        public IReadOnlyCollection<Song> Map(SearchResult searchResult)
        {
            var songs = new List<Song>();

            foreach (var item in searchResult.Tracks.Items)
            {
                // Use the URL as the ID.
                songs.Add(new Song(item.Name, item.Uri, [.. item.Artists.Select(Map)]));
            }

            return songs;
        }

        /// <summary>
        /// Maps a Spotify artist to a generic streaming platform artist model.
        /// </summary>
        /// <param name="artist">A Spotify artist.</param>
        /// <returns>The equivalent <see cref="Artist"/>.</returns>
        public Artist Map(SpotifyArtist artist) 
        {
            return new Artist(artist.Name);
        }
    }
}
