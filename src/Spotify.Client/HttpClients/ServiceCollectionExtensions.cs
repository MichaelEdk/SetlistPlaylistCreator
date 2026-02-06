using Microsoft.Extensions.DependencyInjection;
using Spotify.Client.Playlists;

namespace Spotify.Client.HttpClients
{
    /// <summary>
    /// Extension methods for registering Spotify HTTP clients in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configurtes and registers the Spotify HTTP clients with the dependency injection container.
        /// </summary>
        /// <param name="services">The current instance of <see cref="IServiceCollection"/>.</param>
        /// <returns>A configured instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSpotifyHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient(nameof(AccessTokenHttpClient), client =>
            {
                client.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            // The search, playlist, and user HTTP clients all share the same base address.
            // Consider refactoring to use a single named HTTP client for all three if additional configuration is not needed for each client.
            // I have left them separate for now so there's no magic value needed to be known by each consumer to get the right HTTP client.
            services.AddHttpClient(nameof(SearchHttpClient), client =>
            {
                client.BaseAddress = new Uri("https://api.spotify.com/v1/");
            });

            services.AddHttpClient(nameof(PlaylistHttpClient), client =>
            {
                client.BaseAddress = new Uri("https://api.spotify.com/v1/");
            });

            services.AddHttpClient(nameof(UserHttpClient), client =>
            {
                client.BaseAddress = new Uri("https://api.spotify.com/v1/");
            });

            return services;
        }
    }
}
