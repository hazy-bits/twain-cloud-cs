namespace HazyBits.Twain.Cloud.Registration
{
    /// <summary>
    /// Poll response payload.
    /// </summary>
    public class PollResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether registration was successfully completed by user.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if registration is not successfull.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets device authorization token in case of successfull registration.
        /// </summary>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// Gets or sets device refresh token in case of successfull registration.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}