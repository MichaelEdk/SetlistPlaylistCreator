using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client.Authorization;
using System.Diagnostics;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    public class AuthorizationViewModel : ViewModelBase
    {
        private readonly IAuthorizationCodeUrlBuilder _authorizationCodeUrlBuilder;
        private readonly SpotifyOptions _secrets;

        private Uri _authorizationCodeUrl;
        private string _authorizationButtonText = "Authorize with Spotify";

        public AuthorizationViewModel(
            IAuthorizationCodeUrlBuilder authorizationCodeUrlBuilder,
            IOptions<SpotifyOptions> secrets)
        {
            ArgumentNullException.ThrowIfNull(authorizationCodeUrlBuilder, nameof(authorizationCodeUrlBuilder));
            ArgumentNullException.ThrowIfNull(secrets, nameof(secrets));

            _authorizationCodeUrlBuilder = authorizationCodeUrlBuilder;
            _secrets = secrets.Value;
        }

        public event EventHandler AuthorizationComplete;

        /// <summary>
        /// Gets or sets the text on the authorization button.
        /// </summary>
        public string AuthorizationButtonText
        {
            get => _authorizationButtonText;
            set => RaiseAndSetIfChanged(ref _authorizationButtonText, value, nameof(AuthorizationButtonText));
        }

        /// <summary>
        /// Gets a command that kicks off the Spotify authorization process.
        /// </summary>
        public ICommand Authorize => new RelayCommand<string>(_ => InitializeAuthorizationProcess());

        public void InitializeAuthorizationProcess()
        {
            var url = _authorizationCodeUrlBuilder.BuildUri(_secrets.ClientId, _secrets.RedirectAddress);

            // Open the default browser with the Spotify authorization URL.
            // The user follows the flow, which will then redirect to the redirect address configured in Spotify.
            // This address should have a custom URL protocol handler, which opens this application and passes the token in as
            // a command line parameter.
            Process.Start(new ProcessStartInfo(url.AbsoluteUri)
            {
                UseShellExecute = true
            });

            AuthorizationButtonText = "Waiting for authorization...";
        }
    }
}
