namespace Spotify.Client.Authorization
{
    public class AuthorizationCodeUrlBuilder : IAuthorizationCodeUrlBuilder
    {
        private const string AuthEndpoint = "https://accounts.spotify.com/authorize";
        private const string Scopes = "user-read-private user-read-email playlist-modify-public playlist-modify-private";
        private const string CodeChallengeMethod = "S256";

        private readonly ICodeVerifierGenerator _codeVerifierGenerator;
        private readonly IHashGenerator _hashGenerator;
        private readonly ICodeChallengeStore _codeChallengeStore;

        public AuthorizationCodeUrlBuilder(
            ICodeVerifierGenerator codeVerifierGenerator,
            IHashGenerator hashGenerator,
            ICodeChallengeStore codeChallengeStore)
        {
            _codeVerifierGenerator = codeVerifierGenerator;
            _hashGenerator = hashGenerator;
            _codeChallengeStore = codeChallengeStore;
        }

        public Uri BuildUri(string clientId, string redirectUri)
        {
            var codeVerifier = _codeVerifierGenerator.GenerateRandomString();
            var codeChallenge = _hashGenerator.GenerateHash(codeVerifier);

            _codeChallengeStore.StoreChallenge(codeVerifier);

            return new Uri($"{AuthEndpoint}?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(Scopes)}&code_challenge={Uri.EscapeDataString(codeChallenge)}&code_challenge_method={CodeChallengeMethod}");
        }
    }
}
