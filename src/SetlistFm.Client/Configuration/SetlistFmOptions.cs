namespace SetlistFm.Client.Configuration
{
    /// <summary>
    /// Configuration options related to the SetlistFm API.
    /// </summary>
    public class SetlistFmOptions
    {
        /// <summary>
        /// The name of the configuration section.
        /// </summary>
        public const string Name = "SetlistFm";

        /// <summary>
        /// Gets or sets the API key used to talk to the SetlistFm API.
        /// </summary>
        public string ApiKey { get; set; } = "";
    }
}
