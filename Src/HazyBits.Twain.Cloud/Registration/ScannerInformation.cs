namespace HazyBits.Twain.Cloud.Registration
{
    /// <summary>
    /// Scanner information fields.
    /// </summary>
    public class ScannerInformation
    {
        /// <summary>
        /// Gets or sets the identifier of the device.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the device.
        /// </summary>
        public string Description { get; set; }

        
        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        public string Type { get; set; } // TODO: replace by enum                
        /// <summary>
        /// Gets or sets the version of the device.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the device.
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// Gets or sets the model of the device.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Gets or sets the firmware of the device.
        /// </summary>
        public string Firmware { get; set; }

        /// <summary>
        /// Gets or sets the state of the device.
        /// </summary>
        public string device_state { get; set; }

        /// <summary>
        /// Gets or sets the state of the connection.
        /// </summary>
        public string connection_state { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        public string serial_number { get; set; }

        /// <summary>
        /// Gets or sets the setup URL.
        /// </summary>
        public string setup_url { get; set; }

        /// <summary>
        /// Gets or sets the support URL.
        /// </summary>
        public string support_url { get; set; }

        /// <summary>
        /// Gets or sets the update URL.
        /// </summary>
        public string update_url { get; set; }

        /// <summary>
        /// Gets or sets the state of the semantic.
        /// </summary>
        public string semantic_state { get; set; }

        /// <summary>
        /// Gets or sets the uptime.
        /// </summary>
        public string uptime { get; set; }
    }
}
