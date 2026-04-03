using SetlistPlaylistCreator.Domain;
using SetlistPlaylistCreator.StreamingPlatform;
using Playlist = SetlistPlaylistCreator.StreamingPlatform.Playlist;

namespace SetlistPlaylistCreator.Service
{
    /// <summary>
    /// The default implementation of <see cref="ISetlistPlaylistCreatorService"/>.
    /// </summary>
    public class SetlistPlaylistCreatorService
        : ISetlistPlaylistCreatorService
    {
        private readonly IPlatformSearch _platformSearch;
        private readonly IPlaylistCreator _playlistCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetlistPlaylistCreatorService"/> class.
        /// </summary>
        /// <param name="platformSearch">Component used to search the streaming platform.</param>
        /// <param name="playlistCreator">Component used to create playlists.</param>
        public SetlistPlaylistCreatorService(
            IPlatformSearch platformSearch,
            IPlaylistCreator playlistCreator)
        {
            ArgumentNullException.ThrowIfNull(platformSearch, nameof(platformSearch));
            ArgumentNullException.ThrowIfNull(playlistCreator, nameof(playlistCreator));

            _platformSearch = platformSearch;
            _playlistCreator = playlistCreator;
        }

        /// <inheritdoc />
        public async Task<bool> CreatePlaylistAsync(string playlistName, IReadOnlyCollection<StreamingPlatformSong> songs)
        {
            var playlistSongs = DomainMapper.Map(songs);

            var playlist = new Playlist(playlistName, playlistSongs);

            try
            {
                await _playlistCreator.CreatePlaylistAsync(playlist).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred while creating the playlist: {ex.Message}");
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<SearchedSong>> ProposePlaylistAsync(Setlist setlist)
        {
            var allSongs = setlist.Sets.SelectMany(set => set.Songs);
            var streamingPlatformSetlist = new List<SearchedSong>();

            foreach (var song in allSongs)
            {
                var streamingPlatformSongs = await _platformSearch.SearchSongsAsync(song.Name, song.ArtistName).ConfigureAwait(false);

                if (!streamingPlatformSongs.Any())
                {
                    continue;
                }

                var allMatches = DomainMapper.Map(streamingPlatformSongs);
                streamingPlatformSetlist.Add(new SearchedSong(allMatches.First(), allMatches, song.Name, song.ArtistName));
            }

            return streamingPlatformSetlist;
        }
    }
}
