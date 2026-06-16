namespace Spotify.Client;

/// <summary>
/// An exception thrown when an error occurs while calling the Spotify API.
/// </summary>
public class SpotifyApiException
    : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpotifyApiException"/> class.
    /// </summary>
    /// <param name="message">A message describing the error.</param>
    public SpotifyApiException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpotifyApiException"/> class.
    /// </summary>
    /// <param name="message">A message describing the exception.</param>
    /// <param name="innerException">The inner exception.</param>
    public SpotifyApiException(string? message, Exception? innerException) : base(message, innerException) { }
}
