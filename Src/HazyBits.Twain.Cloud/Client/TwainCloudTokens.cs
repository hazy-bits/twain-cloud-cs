namespace HazyBits.Twain.Cloud.Client
{
    /// <summary>
    /// A set of tokens required to access TWAIN Cloud infrastructure.
    /// </summary>
    public class TwainCloudTokens
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainCloudTokens"/> class.
        /// </summary>
        /// <param name="authorizationToken">The authorization token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        public TwainCloudTokens(string authorizationToken, string refreshToken)
        {
            AuthorizationToken = authorizationToken;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// Gets or sets authorization token which should be passed along with each TWAIN Cloud request.
        /// </summary>
        public string AuthorizationToken { get; }

        /// <summary>
        /// Gets or sets refresh token that can be used to retrieve new authorization token, when the old one expires.
        /// </summary>
        public string RefreshToken { get; }
    }
}
