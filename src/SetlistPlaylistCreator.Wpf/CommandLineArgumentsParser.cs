using System.Text.RegularExpressions;

namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// The default implementation of <see cref="ICommandLineArgumentsParser"/> that parses the command line arguments
    /// </summary>
    internal partial class CommandLineArgumentsParser
        : ICommandLineArgumentsParser
    {
        /// <inheritdoc />
        public CommandLineArguments Parse(string[] args)
        {
            var spotifyRedirectArgument = args[1];
            var match = SpotifyUrlToken().Match(spotifyRedirectArgument);

            return new CommandLineArguments()
            {
                Token = match.Groups["spotifytoken"].Value
            };
        }

        [GeneratedRegex(@"(\w+)?code=(?<spotifytoken>.*)")]
        private static partial Regex SpotifyUrlToken();
    }
}
