using System;

namespace HazyBits.Twain.Cloud.Client
{
    /// <summary>
    /// Event payload for <see cref="TwainCloudClient.TokensRefreshed"/> event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TokensRefreshedEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the updated TWAIN Cloud tokens.
        /// </summary>
        public TwainCloudTokens Tokens { get; set; }
    }
}
