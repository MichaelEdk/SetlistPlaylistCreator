using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using System.Windows;
using Spotify.Client.Authorization;
using SetlistPlaylistCreator.SetlistPlatform;
using SetlistPlaylistCreator.SetlistPlatform.SetlistFm;
using SetlistFm.Client.Configuration;
using SetlistFm.Client;
using SetlistPlaylistCreator.Wpf.SongList;
using SetlistPlaylistCreator.Service;
using SetlistPlaylistCreator.StreamingPlatform;
using SetlistPlaylistCreator.StreamingPlatform.Spotify;
using Spotify.Client;
using SetlistPlaylistCreator.WebApi.Configuration;
using SetlistPlaylistCreator.Wpf.Playlist;
using Spotify.Client.Playlists;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var args = Environment.GetCommandLineArgs();

            // The first argument is the executable name, so we can ignore it.
            // The second argument, if present, is the Spotify redirect URI with the authorization token.
            if (args.Length > 1)
            {
                var passthroughServices = new ServiceCollection()
                    .AddScoped<IAuthorizationCodeStore, DpapiAuthorizationCodeStore>()
                    .AddScoped<ICommandLineArgumentsParser, CommandLineArgumentsParser>()
                    .BuildServiceProvider();

                var tokenStore = passthroughServices.GetRequiredService<IAuthorizationCodeStore>();
                var commandLineArgumentsParser = passthroughServices.GetRequiredService<ICommandLineArgumentsParser>();

                var commandLineArguments = commandLineArgumentsParser.Parse(args);

                tokenStore.StoreCode(commandLineArguments.Token);

                // Once the authorization code is written, exit the application.
                // There's a file system watcher that should notify the existing application that
                // it's time to move onto the next stage.
                Environment.Exit(0);
            }

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<SpotifyOptions>()
                .AddUserSecrets<SetlistFmOptions>()
                .Build();

            var services = new ServiceCollection()
               .AddOptions()
               .AddScoped<IAuthorizationCodeUrlBuilder, AuthorizationCodeUrlBuilder>()
               .AddScoped<ICodeVerifierGenerator, CodeVerifierGenerator>()
               .AddScoped<IHashGenerator, Sha256HashGenerator>()
               .AddSingleton<ICodeChallengeStore, InMemoryCodeChallengeStore>()
               .AddScoped<MainWindow>()
               .AddScoped<ArtistSearchViewModel>()
               .AddScoped<AuthorizationViewModel>()
               .AddScoped<MainWindowViewModel>()
               .AddScoped<SongListViewModel>()
               .AddScoped<PlaylistViewModel>()
               .AddScoped<ISetlistSearch, SetlistFmSetlistSearch>()
               .AddScoped<ISetlistSearchClient, SetlistSearchClient>()
               .AddScoped<IAuthorizationCodeStore, DpapiAuthorizationCodeStore>()
               .AddScoped<ISetlistPlaylistCreatorService, SetlistPlaylistCreatorService>()
               .AddScoped<IPlatformSearch, SpotifyPlatformSearch>()
               .AddScoped<ISearchHttpClient, SearchHttpClient>()
               .AddScoped<IPlaylistHttpClient, PlaylistHttpClient>()
               .AddScoped<IUserHttpClient, UserHttpClient>()
               .AddScoped<IAccessTokenHttpClient, AccessTokenHttpClient>()
               .AddScoped<IPlaylistCreator, SpotifyPlaylistCreator>()
               .AddScoped<IAccessTokenProvider, AuthorizationCodeAccessTokenProvider>()
               .AddScoped<IExternalBrowserLauncher, ExternalBrowserLauncher>()
               .Configure<SetlistFmOptions>(configuration.GetSection(SetlistFmOptions.Name))
               .Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Name))
               .BuildServiceProvider();

            var mainWindow = services.GetRequiredService<MainWindow>();
            var mainWindowViewModel = services.GetRequiredService<MainWindowViewModel>();
            mainWindow.SetMainWindowViewModel(mainWindowViewModel);
            mainWindowViewModel.Initialize();
            mainWindow?.Show();
        }
    }
}