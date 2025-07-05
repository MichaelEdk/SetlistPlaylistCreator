using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client.Authorization;
using System.Windows.Input;

namespace SetlistPlaylistCreator.Wpf.Authorization
{
    public class AuthorizationViewModel : ViewModelBase
    {
        private readonly IAuthorizationCodeUrlBuilder _authorizationCodeUrlBuilder;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly SpotifyOptions _secrets;


        private Uri _authorizationCodeUrl;
        private string? _token = "Enter token here";

        public event EventHandler AuthorizationComplete;

        public Uri AuthorizationCodeUrl
        {
            get => _authorizationCodeUrl;
            set => RaiseAndSetIfChanged(ref _authorizationCodeUrl, value, nameof(AuthorizationCodeUrl));
        }

        public string? Token
        {
            get => _token;
            set => RaiseAndSetIfChanged(ref _token, value, nameof(Token));
        }

        public ICommand InitializeCommand => new RelayCommand<string>(_ => Initialize());

        public ICommand TokenRetrievedCommand => new RelayCommand<string>(_ =>
        {
            _authorizationCodeStore.StoreCode(Token);
            AuthorizationComplete?.Invoke(this, EventArgs.Empty);
        });

        public AuthorizationViewModel(
            IAuthorizationCodeUrlBuilder authorizationCodeUrlBuilder,
            IOptions<SpotifyOptions> secrets,
            IAuthorizationCodeStore authorizationCodeStore)
        {
            _authorizationCodeUrlBuilder = authorizationCodeUrlBuilder;
            _secrets = secrets.Value; ;
            _authorizationCodeStore = authorizationCodeStore;
        }

        public void Initialize()
        {
            // TODO: Register custom handler to start the app.
            var url = _authorizationCodeUrlBuilder.BuildUri(_secrets.ClientId, _secrets.RedirectAddress);

            AuthorizationCodeUrl = url;
        }
    }
}
