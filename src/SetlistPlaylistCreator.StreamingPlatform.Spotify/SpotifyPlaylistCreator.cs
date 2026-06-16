using Spotify.Client;
using Spotify.Client.Playlists;

namespace SetlistPlaylistCreator.StreamingPlatform.Spotify
{
    /// <summary>
    /// The Spotify implementation of <see cref="IPlaylistCreator"/>. This class is responsible for creating playlists on Spotify based on a given <see cref="Playlist"/>.
    /// </summary>
    public class SpotifyPlaylistCreator
        : IPlaylistCreator
    {
        private readonly ISearchHttpClient _searchClient;
        private readonly IPlaylistHttpClient _playlistClient;
        private readonly IUserHttpClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyPlaylistCreator"/> class.
        /// </summary>
        /// <param name="searchClient">An HTTP client used for searching for songs.</param>
        /// <param name="playlistClient">An HTTP client used to handling playlists.</param>
        /// <param name="userClient">An HTTP client used for dealing with Spotify users.</param>
        public SpotifyPlaylistCreator(
            ISearchHttpClient searchClient,
            IPlaylistHttpClient playlistClient,
            IUserHttpClient userClient)
        {
            ArgumentNullException.ThrowIfNull(searchClient, nameof(searchClient));
            ArgumentNullException.ThrowIfNull(playlistClient, nameof(playlistClient));
            ArgumentNullException.ThrowIfNull(userClient, nameof(userClient));

            _searchClient = searchClient;
            _playlistClient = playlistClient;
            _userClient = userClient;
        }

        /// <inheritdoc />
        public async Task CreatePlaylistAsync(Playlist playlist)
        {
            var uris = new List<string>();

            foreach (var song in playlist.Songs)
            {
                // If we have the ID, use that. If not, search the streaming platform.
                // Searching the streaming platform shouldn't be necessary. I just
                // coded it that way first and left it.
                if (song.Id is not null)
                {
                    uris.Add(song.Id);
                }
                else
                {
                    var searchResult = await _searchClient.SearchSongsAsync(song.Title, song.Artists.First().Name);
                    var track = searchResult.Tracks.Items.FirstOrDefault();

                    if (track != null)
                    {
                        uris.Add(track.Uri);
                    }
                }

            }

            var user = await _userClient.GetUserProfileAsync();

            // Create an empty playlist.
            var playlistId = await _playlistClient.CreatePlaylistAsync(user.Id, playlist.Name);

            // Add the songs to the playlist.
            await _playlistClient.AddSongsToPlaylist(playlistId, uris);
        }
    }
}
