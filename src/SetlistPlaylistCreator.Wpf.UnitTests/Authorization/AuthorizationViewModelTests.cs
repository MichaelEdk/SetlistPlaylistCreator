using Microsoft.Extensions.Options;
using SetlistPlaylistCreator.WebApi.Configuration;
using SetlistPlaylistCreator.Wpf.Authorization;
using Spotify.Client.Authorization;

namespace SetlistPlaylistCreator.Wpf.UnitTests.Authorization
{
    [TestClass]
    public class AuthorizationViewModelTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Act
            var viewModel = CreateAuthorizationViewModel();

            // Assert that the text is correct before authorizing with Spotify.
            Assert.AreEqual("Authorize with Spotify", viewModel.AuthorizationButtonText);
            Assert.IsNotNull(viewModel.Authorize);
        }

        [TestMethod]
        public void Constructor_WithNullAuthorizationCodeUrlBuilder_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var secrets = CreateSpotifyOptions();
            var authorizationCodeFileWatcher = Substitute.For<IAuthorizationCodeFileWatcher>();

            // Act & Assert
            var actualException = Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    null!,
                    authorizationCodeStore,
                    externalBrowserLauncher,
                    secrets,
                    authorizationCodeFileWatcher));

            Assert.AreEqual("authorizationCodeUrlBuilder", actualException.ParamName);
        }

        [TestMethod]
        public void Constructor_WithNullAuthorizationCodeStore_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var secrets = CreateSpotifyOptions();
            var authorizationCodeFileWatcher = Substitute.For<IAuthorizationCodeFileWatcher>();

            // Act & Assert
            var actualException = Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    null!,
                    externalBrowserLauncher,
                    secrets,
                    authorizationCodeFileWatcher));

            Assert.AreEqual("authorizationCodeStore", actualException.ParamName);
        }

        [TestMethod]
        public void Constructor_WithNullExternalBrowserLauncher_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var secrets = CreateSpotifyOptions();
            var authorizationCodeFileWatcher = Substitute.For<IAuthorizationCodeFileWatcher>();

            // Act & Assert
            var actualException = Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    authorizationCodeStore,
                    null!,
                    secrets,
                    authorizationCodeFileWatcher));

            Assert.AreEqual("externalBrowserLauncher", actualException.ParamName);
        }

        [TestMethod]
        public void Constructor_WithNullSecrets_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var authorizationCodeFileWatcher = Substitute.For<IAuthorizationCodeFileWatcher>();

            // Act & Assert
            var actualException = Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    authorizationCodeStore,
                    externalBrowserLauncher,
                    null!,
                    authorizationCodeFileWatcher));

            Assert.AreEqual("secrets", actualException.ParamName);
        }

        [TestMethod]
        public void AuthorizationButtonText_WhenSet_RaisesPropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateAuthorizationViewModel();
            var propertyChangedRaised = false;
            string? propertyName = null;

            viewModel.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                propertyName = e.PropertyName;
            };

            // Act
            viewModel.AuthorizationButtonText = "New Text";

            // Assert
            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(nameof(AuthorizationViewModel.AuthorizationButtonText), propertyName);
            Assert.AreEqual("New Text", viewModel.AuthorizationButtonText);
        }

        [TestMethod]
        public void AuthorizationButtonText_WhenSetToSameValue_DoesNotRaisePropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateAuthorizationViewModel();
            var initialText = viewModel.AuthorizationButtonText;
            var propertyChangedRaised = false;

            viewModel.PropertyChanged += (sender, e) => propertyChangedRaised = true;

            // Act
            viewModel.AuthorizationButtonText = initialText;

            // Assert
            Assert.IsFalse(propertyChangedRaised);
        }

        [TestMethod]
        public void Authorize_Execute_CallsUrlBuilderAndLaunchesBrowser()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var secrets = CreateSpotifyOptions("test-client-id", "https://redirect.com");
            var expectedUri = new Uri("https://spotify.com/authorize");
            var authorizationCodeFileWatcher = Substitute.For<IAuthorizationCodeFileWatcher>();

            authorizationCodeUrlBuilder.BuildUri("test-client-id", "https://redirect.com")
                .Returns(expectedUri);

            var viewModel = CreateAuthorizationViewModel(
                authorizationCodeUrlBuilder,
                authorizationCodeStore,
                externalBrowserLauncher,
                secrets,
                authorizationCodeFileWatcher);

            // Act
            viewModel.Authorize.Execute(null);

            // Assert
            authorizationCodeUrlBuilder.Received(1).BuildUri("test-client-id", "https://redirect.com");
            externalBrowserLauncher.Received(1).Launch(expectedUri);
            authorizationCodeFileWatcher.Received(1).Enable();
        }

        [TestMethod]
        public void Authorize_Execute_UpdatesAuthorizationButtonText()
        {
            // Arrange
            var viewModel = CreateAuthorizationViewModel();

            // Act
            viewModel.Authorize.Execute(null);

            // Assert
            Assert.AreEqual("Waiting for authorization...", viewModel.AuthorizationButtonText);
        }

        private static IOptions<SpotifyOptions> CreateSpotifyOptions(string clientId = "test-client-id", string redirectAddress = "https://test-redirect.com")
        {
            var options = new SpotifyOptions
            {
                ClientId = clientId,
                RedirectAddress = redirectAddress,
                ClientSecret = "test-secret"
            };

            var mockOptions = Substitute.For<IOptions<SpotifyOptions>>();
            mockOptions.Value.Returns(options);
            return mockOptions;
        }

        private static AuthorizationViewModel CreateAuthorizationViewModel(
            IAuthorizationCodeUrlBuilder? authorizationCodeUrlBuilder = null,
            IAuthorizationCodeStore? authorizationCodeStore = null,
            IExternalBrowserLauncher? externalBrowserLauncher = null,
            IOptions<SpotifyOptions>? secrets = null,
            IAuthorizationCodeFileWatcher? authorizationCodeFileWatcher = null)
        {
            return new(
                authorizationCodeUrlBuilder ?? Substitute.For<IAuthorizationCodeUrlBuilder>(),
                authorizationCodeStore ?? Substitute.For<IAuthorizationCodeStore>(),
                externalBrowserLauncher ?? Substitute.For<IExternalBrowserLauncher>(),
                secrets ?? CreateSpotifyOptions(),
                authorizationCodeFileWatcher ?? Substitute.For<IAuthorizationCodeFileWatcher>()
            );
        }
    }
}
