using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration.UserSecrets;
using SetlistPlaylistCreator.Wpf.ArtistSearch;
using SetlistPlaylistCreator.Wpf.Authorization;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Options;
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

            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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
               .Configure<SetlistFmOptions>(configuration.GetSection(SetlistFmOptions.Name))
               .Configure<SpotifyOptions>(configuration.GetSection(SpotifyOptions.Name))
               .BuildServiceProvider();

            var mainWindow = services.GetService<MainWindow>();
            var mainWindowViewModel = services.GetService<MainWindowViewModel>();
            mainWindow.SetMainWindowViewModel(mainWindowViewModel);
            mainWindowViewModel.Initialize();
            mainWindow?.Show();

        }
    }
}