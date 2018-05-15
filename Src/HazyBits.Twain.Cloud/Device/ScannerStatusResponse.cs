namespace HazyBits.Twain.Cloud.Device
{
    /// <summary>
    /// Scanner status response payload.
    /// </summary>
    internal class ScannerStatusResponse
    {
        /// <summary>
        /// Gets or sets the URL to use to connect to TWAIN Cloud MQTT broker.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name of MQTT topic to use for incoming messages.
        /// </summary>
        public string RequestTopic { get; set; }

        /// <summary>
        /// Gets or sets the name of MQTT topic to use for outgoing messages.
        /// </summary>
        public string ResponseTopic { get; set; }
    }
}
