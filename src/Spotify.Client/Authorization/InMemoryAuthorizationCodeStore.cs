namespace Spotify.Client.Authorization
{
    /// <summary>
    /// The default implementation of <see cref="IAuthorizationCodeStore"/> that stores the authorization code in memory.
    /// </summary>
    public class InMemoryAuthorizationCodeStore
        : IAuthorizationCodeStore
    {
        private string? _code;

        /// <inheritdoc />
        public void ClearStore()
        {
            _code = null;
        }

        /// <inheritdoc />
        public string? RetrieveCode()
        {
            return _code;
        }

        /// <inheritdoc />
        public void StoreCode(string code)
        {
            _code = code;
        }
    }
}
