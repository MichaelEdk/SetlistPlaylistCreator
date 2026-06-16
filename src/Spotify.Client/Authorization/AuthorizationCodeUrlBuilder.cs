using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Spotify.Client.Authorization
{
    /// <summary>
    /// The default implementation of <see cref="IAuthorizationCodeUrlBuilder"/>.
    /// </summary>
    public class AuthorizationCodeUrlBuilder
        : IAuthorizationCodeUrlBuilder
    {
        private const string AuthEndpoint = "https://accounts.spotify.com/authorize";
        private const string Scopes = "user-read-private user-read-email playlist-modify-public playlist-modify-private";
        private const string CodeChallengeMethod = "S256";

        private readonly ICodeVerifierGenerator _codeVerifierGenerator;
        private readonly IHashGenerator _hashGenerator;
        private readonly ICodeChallengeStore _codeChallengeStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeUrlBuilder"/> class.
        /// </summary>
        /// <param name="codeVerifierGenerator">The code verifier generator used to create a random code verifier for PKCE.</param>
        /// <param name="hashGenerator">The hash generator used to create a code challenge from the code verifier.</param>
        /// <param name="codeChallengeStore">The store used to persist the code verifier for later retrieval during the OAuth 2.0 authorization flow.</param>
        public AuthorizationCodeUrlBuilder(
            ICodeVerifierGenerator codeVerifierGenerator,
            IHashGenerator hashGenerator,
            ICodeChallengeStore codeChallengeStore)
        {
            ArgumentNullException.ThrowIfNull(codeVerifierGenerator, nameof(codeVerifierGenerator));
            ArgumentNullException.ThrowIfNull(hashGenerator, nameof(hashGenerator));
            ArgumentNullException.ThrowIfNull(codeChallengeStore, nameof(codeChallengeStore));

            _codeVerifierGenerator = codeVerifierGenerator;
            _hashGenerator = hashGenerator;
            _codeChallengeStore = codeChallengeStore;
        }

        /// <inheritdoc />
        public Uri BuildUri(string clientId, string redirectUri)
        {
            var codeVerifier = _codeVerifierGenerator.GenerateRandomString();
            var codeChallenge = _hashGenerator.GenerateHash(codeVerifier);

            _codeChallengeStore.StoreChallenge(codeVerifier);

            var uriWithQueryString = QueryHelpers.AddQueryString(AuthEndpoint, new List<KeyValuePair<string, StringValues>>
            {
                { new KeyValuePair<string, StringValues>("response_type", "code") },
                { new KeyValuePair<string, StringValues>("client_id", clientId) },
                { new KeyValuePair<string, StringValues>("redirect_uri", redirectUri) },
                { new KeyValuePair<string, StringValues>("scope", Scopes) },
                { new KeyValuePair<string, StringValues>("code_challenge", codeChallenge) },
                { new KeyValuePair<string, StringValues>("code_challenge_method", CodeChallengeMethod) },
            });

            return new Uri(uriWithQueryString);
        }
    }
}
