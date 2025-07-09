namespace SetlistPlaylistCreator.Wpf
{
    /// <summary>
    /// Parses command line arguments for the application.
    /// </summary>
    internal interface ICommandLineArgumentsParser
    {
        /// <summary>
        /// Parses the command line arguments and returns a <see cref="CommandLineArguments"/> object.
        /// </summary>
        /// <param name="args">The command line arguments passed into the application.</param>
        /// <returns>A <see cref="CommandLineArguments"/> object based on the arguments passed into the application.</returns>
        CommandLineArguments Parse(string[] args);
    }
}