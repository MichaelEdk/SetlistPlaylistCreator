using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SetlistFm.Client;
using SetlistFm.Client.Configuration;

namespace SetlistFm.TestApplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var configurationBuilder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            var configurationRoot = configurationBuilder.Build();

            builder.Services.AddSingleton<ISetlistSearchClient, SetlistSearchClient>();
            builder.Services.AddHttpClient();
            // Get the API key from configuration.
            builder.Services.Configure<SetlistFmOptions>(configurationRoot.GetSection(SetlistFmOptions.Name));

            using var host = builder.Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var searchClient = services.GetRequiredService<ISetlistSearchClient>();

            Console.WriteLine("Enter an artist's name:");
            var artistName = Console.ReadLine();

            var result = await searchClient.SearchSetlistsAsync(artistName, CancellationToken.None).ConfigureAwait(true);

            foreach (var setlist in result.Setlist)
            {
                foreach (var set in setlist.Sets.Set)
                {
                    Console.WriteLine($"Set name: {set.Name}");

                    foreach (var song in set.Song)
                    {
                        Console.WriteLine($"{song.Name}");
                    }
                }
            }

            host.Run();
        }
    }
}