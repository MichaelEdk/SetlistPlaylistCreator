using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SetlistFm.Client;
using SetlistFm.Client.Configuration;
using SetlistPlaylistCreator.Service;
using SetlistPlaylistCreator.SetlistPlatform;
using SetlistPlaylistCreator.SetlistPlatform.SetlistFm;
using SetlistPlaylistCreator.StreamingPlatform;
using SetlistPlaylistCreator.StreamingPlatform.Spotify;
using SetlistPlaylistCreator.WebApi.Configuration;
using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using SetlistPlaylistCreator.Wpf.Complete;
using SetlistPlaylistCreator.Wpf.Playlist;
using SetlistPlaylistCreator.Wpf.SongList;
using Spotify.Client;
using Spotify.Client.Authorization;
using Spotify.Client.HttpClients;
using Spotify.Client.Playlists;
using System.Windows;

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
               .AddScoped<CompleteViewModel>()
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
               .AddScoped<IAuthorizationCodeFileWatcher, AuthorizationCodeFileWatcher>()
               .Configure<SetlistFmOptions>(configuration.GetSection(SetlistFmOptions.Name))
               .Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Name))
               .AddSpotifyHttpClients()
               .BuildServiceProvider();

            var mainWindow = services.GetRequiredService<MainWindow>();
            var mainWindowViewModel = services.GetRequiredService<MainWindowViewModel>();
            mainWindow.SetMainWindowViewModel(mainWindowViewModel);
            mainWindowViewModel.Initialize();
            mainWindow?.Show();
        }
    }
}