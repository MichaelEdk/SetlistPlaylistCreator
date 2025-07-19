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
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var secrets = CreateSpotifyOptions("test-client-id", "https://redirect.com");

            // Act
            var viewModel = CreateSubjectUnderTest(
                authorizationCodeUrlBuilder,
                authorizationCodeStore,
                externalBrowserLauncher,
                secrets);

            // Assert
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

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    null!,
                    authorizationCodeStore,
                    externalBrowserLauncher,
                    secrets));
        }

        [TestMethod]
        public void Constructor_WithNullAuthorizationCodeStore_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();
            var secrets = CreateSpotifyOptions();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    null!,
                    externalBrowserLauncher,
                    secrets));
        }

        [TestMethod]
        public void Constructor_WithNullExternalBrowserLauncher_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var secrets = CreateSpotifyOptions();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    authorizationCodeStore,
                    null!,
                    secrets));
        }

        [TestMethod]
        public void Constructor_WithNullSecrets_ThrowsArgumentNullException()
        {
            // Arrange
            var authorizationCodeUrlBuilder = Substitute.For<IAuthorizationCodeUrlBuilder>();
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            var externalBrowserLauncher = Substitute.For<IExternalBrowserLauncher>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthorizationViewModel(
                    authorizationCodeUrlBuilder,
                    authorizationCodeStore,
                    externalBrowserLauncher,
                    null!));
        }

        [TestMethod]
        public void AuthorizationButtonText_WhenSet_RaisesPropertyChangedEvent()
        {
            // Arrange
            var viewModel = CreateSubjectUnderTest();
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
            var viewModel = CreateSubjectUnderTest();
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

            authorizationCodeUrlBuilder.BuildUri("test-client-id", "https://redirect.com")
                .Returns(expectedUri);

            var viewModel = CreateSubjectUnderTest(
                authorizationCodeUrlBuilder,
                authorizationCodeStore,
                externalBrowserLauncher,
                secrets);

            // Act
            viewModel.Authorize.Execute(null);

            // Assert
            authorizationCodeUrlBuilder.Received(1).BuildUri("test-client-id", "https://redirect.com");
            externalBrowserLauncher.Received(1).Launch(expectedUri);
        }

        [TestMethod]
        public void Authorize_Execute_UpdatesAuthorizationButtonText()
        {
            // Arrange
            var viewModel = CreateSubjectUnderTest();

            // Act
            viewModel.Authorize.Execute(null);

            // Assert
            Assert.AreEqual("Waiting for authorization...", viewModel.AuthorizationButtonText);
        }

        [TestMethod]
        public void FileWatcher_Changed_WithValidAuthorizationCode_RaisesAuthorizationCompleteEvent()
        {
            // Arrange
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            authorizationCodeStore.RetrieveCode().Returns("valid-auth-code");

            var viewModel = CreateSubjectUnderTest(authorizationCodeStore: authorizationCodeStore);
            var authorizationCompleteRaised = false;

            viewModel.AuthorizationComplete += (sender, e) => authorizationCompleteRaised = true;

            // Act
            // Simulate file change by using reflection to access the private method
            var fileWatcherField = typeof(AuthorizationViewModel)
                .GetField("_authorizationCodeFileWatcher", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fileWatcher = (FileSystemWatcher)fileWatcherField!.GetValue(viewModel)!;
            
            // Trigger the Changed event
            var args = new FileSystemEventArgs(WatcherChangeTypes.Changed, EncryptedTokenFile.Directory, EncryptedTokenFile.FileName);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            
            // Use reflection to call the private FileWatcher_Changed method
            var method = typeof(AuthorizationViewModel)
                .GetMethod("FileWatcher_Changed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(viewModel, new object[] { fileWatcher, args });

            // Assert
            Assert.IsTrue(authorizationCompleteRaised);
            authorizationCodeStore.Received(1).RetrieveCode();
        }

        [TestMethod]
        public void FileWatcher_Changed_WithEmptyAuthorizationCode_DoesNotRaiseAuthorizationCompleteEvent()
        {
            // Arrange
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            authorizationCodeStore.RetrieveCode().Returns("");

            var viewModel = CreateSubjectUnderTest(authorizationCodeStore: authorizationCodeStore);
            var authorizationCompleteRaised = false;

            viewModel.AuthorizationComplete += (sender, e) => authorizationCompleteRaised = true;

            // Act
            // Use reflection to call the private FileWatcher_Changed method
            var method = typeof(AuthorizationViewModel)
                .GetMethod("FileWatcher_Changed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var args = new FileSystemEventArgs(WatcherChangeTypes.Changed, EncryptedTokenFile.Directory, EncryptedTokenFile.FileName);
            method!.Invoke(viewModel, new object?[] { null, args });

            // Assert
            Assert.IsFalse(authorizationCompleteRaised);
        }

        [TestMethod]
        public void FileWatcher_Changed_WithNullAuthorizationCode_DoesNotRaiseAuthorizationCompleteEvent()
        {
            // Arrange
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            authorizationCodeStore.RetrieveCode().Returns((string?)null);

            var viewModel = CreateSubjectUnderTest(authorizationCodeStore: authorizationCodeStore);
            var authorizationCompleteRaised = false;

            viewModel.AuthorizationComplete += (sender, e) => authorizationCompleteRaised = true;

            // Act
            // Use reflection to call the private FileWatcher_Changed method
            var method = typeof(AuthorizationViewModel)
                .GetMethod("FileWatcher_Changed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var args = new FileSystemEventArgs(WatcherChangeTypes.Changed, EncryptedTokenFile.Directory, EncryptedTokenFile.FileName);
            method!.Invoke(viewModel, new object?[] { null, args });

            // Assert
            Assert.IsFalse(authorizationCompleteRaised);
        }

        [TestMethod]
        public void FileWatcher_Changed_WithWhitespaceAuthorizationCode_DoesNotRaiseAuthorizationCompleteEvent()
        {
            // Arrange
            var authorizationCodeStore = Substitute.For<IAuthorizationCodeStore>();
            authorizationCodeStore.RetrieveCode().Returns("   ");

            var viewModel = CreateSubjectUnderTest(authorizationCodeStore: authorizationCodeStore);
            var authorizationCompleteRaised = false;

            viewModel.AuthorizationComplete += (sender, e) => authorizationCompleteRaised = true;

            // Act
            // Use reflection to call the private FileWatcher_Changed method
            var method = typeof(AuthorizationViewModel)
                .GetMethod("FileWatcher_Changed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var args = new FileSystemEventArgs(WatcherChangeTypes.Changed, EncryptedTokenFile.Directory, EncryptedTokenFile.FileName);
            method!.Invoke(viewModel, new object?[] { null, args });

            // Assert
            Assert.IsFalse(authorizationCompleteRaised);
        }

        [TestMethod]
        public void Authorize_CanExecute_ReturnsTrue()
        {
            // Arrange
            var viewModel = CreateSubjectUnderTest();

            // Act
            var canExecute = viewModel.Authorize.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void InitializeAuthorizationProcess_EnablesFileWatcherRaisingEvents()
        {
            // Arrange
            var viewModel = CreateSubjectUnderTest();

            // Get the file watcher using reflection
            var fileWatcherField = typeof(AuthorizationViewModel)
                .GetField("_authorizationCodeFileWatcher", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fileWatcher = (FileSystemWatcher)fileWatcherField!.GetValue(viewModel)!;

            // Ensure it starts disabled
            fileWatcher.EnableRaisingEvents = false;

            // Act
            viewModel.Authorize.Execute(null);

            // Assert
            Assert.IsTrue(fileWatcher.EnableRaisingEvents);
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

        private static AuthorizationViewModel CreateSubjectUnderTest(
            IAuthorizationCodeUrlBuilder? authorizationCodeUrlBuilder = null,
            IAuthorizationCodeStore? authorizationCodeStore = null,
            IExternalBrowserLauncher? externalBrowserLauncher = null,
            IOptions<SpotifyOptions>? secrets = null)
        {
            return new AuthorizationViewModel(
                authorizationCodeUrlBuilder ?? Substitute.For<IAuthorizationCodeUrlBuilder>(),
                authorizationCodeStore ?? Substitute.For<IAuthorizationCodeStore>(),
                externalBrowserLauncher ?? Substitute.For<IExternalBrowserLauncher>(),
                secrets ?? CreateSpotifyOptions()
            );
        }
    }
}
