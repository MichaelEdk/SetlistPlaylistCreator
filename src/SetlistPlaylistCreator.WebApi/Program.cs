using SetlistPlaylistCreator.StreamingPlatform;
using SetlistPlaylistCreator.StreamingPlatform.Spotify;
using SetlistPlaylistCreator.WebApi.Configuration;
using Spotify.Client;
using Spotify.Client.Authorization;
using Spotify.Client.Playlists;

namespace SetlistPlaylistCreator.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IAccessTokenHttpClient, AccessTokenHttpClient>();
            builder.Services.AddSingleton<ISearchHttpClient, SearchHttpClient>();
            builder.Services.AddSingleton<IUserHttpClient, UserHttpClient>();
            builder.Services.AddSingleton<IAuthorizationCodeUrlBuilder, AuthorizationCodeUrlBuilder>();
            builder.Services.AddSingleton<IHashGenerator, Sha256HashGenerator>();
            builder.Services.AddSingleton<ICodeVerifierGenerator, CodeVerifierGenerator>();
            builder.Services.AddSingleton<IAuthorizationCodeStore, AuthorizationCodeStore>();
            builder.Services.AddSingleton<ICodeChallengeStore, CodeChallengeStore>();
            builder.Services.AddSingleton<IPlaylistHttpClient, PlaylistHttpClient>();

            builder.Services.AddSingleton<IAccessTokenProvider, AuthorizationCodeAccessTokenProvider>();
;
            builder.Services.AddSingleton<IPlaylistCreator, SpotifyPlaylistCreator>();
            builder.Services.AddSingleton<IAuthorizationProvider, SpotifyAuthorizationProvider>();
            builder.Services.AddSingleton<IPlatformSearch, SpotifyPlatformSearch>();

            builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection(SpotifyOptions.Name));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
            });

            app.MapControllers();

            app.Run();
        }
    }
}