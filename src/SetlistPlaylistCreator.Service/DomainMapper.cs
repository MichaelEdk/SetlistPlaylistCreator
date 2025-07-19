using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.StreamingPlatform;

namespace SetlistPlaylistCreator.Service
{
    /// <summary>
    /// Contains mappings between domain objects and streaming platform models.
    /// </summary>
    public class DomainMapper
    {
        /// <summary>
        /// Maps a collection of <see cref="StreamingPlatformSong"/> to a collection of <see cref="Song"/>.
        /// </summary>
        /// <param name="streamingPlatformSongs">A collection of <see cref="StreamingPlatformSong"/>.</param>
        /// <returns>The equivalent collection of <see cref="Song"/>.</returns>
        public static IReadOnlyCollection<Song> Map(IReadOnlyCollection<StreamingPlatformSong> streamingPlatformSongs) =>
            streamingPlatformSongs
                .Select(song => new Song(song.SongName, song.Id, Map(song.ArtistNames)))
                .ToList();

        /// <summary>
        /// Maps a collection of <see cref="Song"/> to a collection of <see cref="StreamingPlatformSong"/>.
        /// </summary>
        /// <param name="songs">A collection of <see cref="Song"/>.</param>
        /// <returns>The equivalent collection of <see cref="StreamingPlatformSong"/>.</returns>
        public static IReadOnlyCollection<StreamingPlatformSong> Map(IEnumerable<Song> songs) =>
            songs
                .Select(song => new StreamingPlatformSong(
                    song.Title,
                    song.Artists.Select(artist => artist.Name).ToList(),
                    song.Id))
                .ToList();

        /// <summary>
        /// Maps a collection of artist names to a collection of <see cref="Artist"/> objects.
        /// </summary>
        /// <param name="artistNames">A collection of artist names.</param>
        /// <returns>The equivalent collection of <see cref="Artist"/>.</returns>
        public static IReadOnlyCollection<Artist> Map(IReadOnlyCollection<string> artistNames) =>
            artistNames
                .Select(artistName => new Artist(artistName))
                .ToList();
    }
}
