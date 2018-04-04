namespace HazyBits.Twain.Cloud.Registration
{
    /// <summary>
    /// Registration reponse payload.
    /// </summary>
    public class RegistrationResponse
    {
        /// <summary>
        /// Gets or sets unique scanner identifier, assigned to the device.
        /// </summary>
        public string ScannerId { get; set; }

        /// <summary>
        /// Gets or sets the registration token that should be used to complete the registration.
        /// </summary>
        public string RegistrationToken { get; set; }

        /// <summary>
        /// Gets or sets the polling URL to retrieve registration status.
        /// </summary>
        public string PollingUrl { get; set; }

        /// <summary>
        /// Gets or sets the invite URL to be used to complete registration process.
        /// </summary>
        public string InviteUrl { get; set; }
    }
}
