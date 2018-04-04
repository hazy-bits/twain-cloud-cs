namespace HazyBits.Twain.Cloud.Events
{
    /// <summary>
    /// MQTT message properties.
    /// </summary>
    internal class MqttMessage
    {
        /// <summary>
        /// Gets or sets the MQTT topic for the message.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public string Message { get; set; }
    }
}