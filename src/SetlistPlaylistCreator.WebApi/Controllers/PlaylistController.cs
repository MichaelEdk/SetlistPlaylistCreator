using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.StreamingPlatform;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client.Authorization;

namespace SetlistPlaylistCreator.WebApi.Controllers
{
    [Route("playlist")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistCreator _playlistCreator;
        private readonly IAuthorizationProvider _authorizationProvider;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly SpotifyOptions _spotifyOptions;

        public PlaylistController(
            IPlaylistCreator playlistCreator,
            IAuthorizationProvider authorizationProvider,
            IAuthorizationCodeStore authorizationCodeStore,
            IOptionsMonitor<SpotifyOptions> spotifyOptions)
        {
            ArgumentNullException.ThrowIfNull(playlistCreator, nameof(playlistCreator));
            ArgumentNullException.ThrowIfNull(authorizationProvider, nameof(authorizationProvider));
            ArgumentNullException.ThrowIfNull(authorizationCodeStore, nameof(authorizationCodeStore));
            ArgumentNullException.ThrowIfNull(spotifyOptions, nameof(spotifyOptions));
            
            _playlistCreator = playlistCreator;
            _authorizationProvider = authorizationProvider;
            _authorizationCodeStore = authorizationCodeStore;
            _spotifyOptions = spotifyOptions.CurrentValue;
        }

        [HttpPost]
        [Route("create")]
        public Task PlaylistCreatorAsync([FromBody] Playlist playlist)
        {
            _authorizationCodeStore.StoreCode(Request.Headers.Authorization.First().Split(" ")[1]);
            return _playlistCreator.CreatePlaylistAsync(playlist);
        }

        [HttpGet]
        [Route("authorize")]
        public Uri GetAuthorizationUrl([FromQuery] string redirectAddress)
        {
            return _authorizationProvider.GetAuthorizationUrl(new Uri(redirectAddress), _spotifyOptions.ClientId);
        }
    }
}
