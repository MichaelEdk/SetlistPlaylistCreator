namespace Spotify.Client;

/// <summary>
/// An exception thrown when there is an error within the Spotify Client library that doesn't relate to calling the Spotify API.
/// </summary>
public class SpotifyClientException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpotifyApiException"/> class.
    /// </summary>
    /// <param name="message">A message describing the error.</param>
    public SpotifyClientException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpotifyApiException"/> class.
    /// </summary>
    /// <param name="message">A message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public SpotifyClientException(string? message, Exception? innerException) : base(message, innerException) { }
}
