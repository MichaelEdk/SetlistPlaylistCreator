namespace Spotify.Client.Authorization
{
    public class AuthorizationCodeStore
        : IAuthorizationCodeStore
    {
        private string? _code;

        public string? RetrieveCode()
        {
            return _code;
        }

        public void StoreCode(string code)
        {
            _code = code;
        }
    }
}
