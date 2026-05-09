using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client.Authorization;
using System.IO;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    /// <summary>
    /// A view model to back the AuthorizationUserControl view.
    /// </summary>
    public class AuthorizationViewModel
        : ViewModelBase
    {
        private readonly IAuthorizationCodeFileWatcher _authorizationCodeFileWatcher;

        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly IAuthorizationCodeUrlBuilder _authorizationCodeUrlBuilder;
        private readonly IExternalBrowserLauncher _externalBrowserLauncher;
        private readonly SpotifyOptions _secrets;

        private string _authorizationButtonText = "Authorize with Spotify";
        private bool _isAuthorizing;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationViewModel"/> class.
        /// </summary>
        /// <param name="authorizationCodeUrlBuilder">Component used to build Spotify authorization URLs.</param>
        /// <param name="authorizationCodeStore">A store for keeping the authorization code used to generate Spotify access tokens.</param>
        /// <param name="externalBrowserLauncher">Launches external browsers.</param>
        /// <param name="secrets">Spotify secret configuration.</param>
        /// <param name="authorizationCodeFileWatcher">Abstraction for watching the authorization code file.</param>
        public AuthorizationViewModel(
            IAuthorizationCodeUrlBuilder authorizationCodeUrlBuilder,
            IAuthorizationCodeStore authorizationCodeStore,
            IExternalBrowserLauncher externalBrowserLauncher,
            IOptions<SpotifyOptions> secrets,
            IAuthorizationCodeFileWatcher authorizationCodeFileWatcher)
        {
            ArgumentNullException.ThrowIfNull(authorizationCodeUrlBuilder, nameof(authorizationCodeUrlBuilder));
            ArgumentNullException.ThrowIfNull(authorizationCodeStore, nameof(authorizationCodeStore));
            ArgumentNullException.ThrowIfNull(externalBrowserLauncher, nameof(externalBrowserLauncher));
            ArgumentNullException.ThrowIfNull(secrets, nameof(secrets));
            ArgumentNullException.ThrowIfNull(authorizationCodeFileWatcher, nameof(authorizationCodeFileWatcher));

            _authorizationCodeUrlBuilder = authorizationCodeUrlBuilder;
            _authorizationCodeStore = authorizationCodeStore;
            _externalBrowserLauncher = externalBrowserLauncher;
            _secrets = secrets.Value;
            _authorizationCodeFileWatcher = authorizationCodeFileWatcher;

            _authorizationCodeFileWatcher.Changed += FileWatcher_Changed;
        }

        /// <summary>
        /// An event raised to signal that the authorization process is complete.
        /// </summary>
        public event EventHandler? AuthorizationComplete;

        /// <summary>
        /// Gets or sets the text on the authorization button.
        /// </summary>
        public string AuthorizationButtonText
        {
            get => _authorizationButtonText;
            set => RaiseAndSetIfChanged(ref _authorizationButtonText, value, nameof(AuthorizationButtonText));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the authorization process is currently in progress.
        /// </summary>
        public bool IsAuthorizing
        {
            get => _isAuthorizing;
            set
            {
                RaiseAndSetIfChanged(ref _isAuthorizing, value, nameof(IsAuthorizing));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Gets a command that kicks off the Spotify authorization process.
        /// </summary>
        public ICommand Authorize => new RelayCommand<string>(
            _ => !IsAuthorizing,
            _ => InitializeAuthorizationProcess());

        private void FileWatcher_Changed(object? sender, FileSystemEventArgs e)
        {
            var authorizationCode = _authorizationCodeStore.RetrieveCode();

            if (!string.IsNullOrWhiteSpace(authorizationCode))
            {
                _authorizationCodeFileWatcher.Changed -= FileWatcher_Changed;
                _authorizationCodeFileWatcher.Disable();

                // This should notify consumers that it is time to move to the next window.
                AuthorizationComplete?.Invoke(this, new EventArgs());
            }
        }

        private void InitializeAuthorizationProcess()
        {
            IsAuthorizing = true;

            var url = _authorizationCodeUrlBuilder.BuildUri(_secrets.ClientId, _secrets.RedirectAddress);

            // Open the default browser with the Spotify authorization URL.
            // The user follows the flow, which will then redirect to the redirect address configured in Spotify.
            // This address should have a custom URL protocol handler, which opens this application and passes the token in as
            // a command line parameter.
            _externalBrowserLauncher.Launch(url);

            _authorizationCodeFileWatcher.Enable();

            AuthorizationButtonText = "Waiting for authorization...";
        }
    }
}
