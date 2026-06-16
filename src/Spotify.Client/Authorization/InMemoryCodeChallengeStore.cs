namespace Spotify.Client.Authorization
{
    /// <summary>
    /// An in-memory implementation of <see cref="ICodeChallengeStore"/>.
    /// </summary>
    public class InMemoryCodeChallengeStore
        : ICodeChallengeStore
    {
        private string _challenge = string.Empty;

        /// <inheritdoc />
        public string RetrieveChallenge()
        {
            return _challenge;
        }

        /// <inheritdoc />
        public void StoreChallenge(string challenge)
        {
            _challenge = challenge;
        }
    }
}
