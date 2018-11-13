namespace HazyBits.Twain.Cloud.Registration
{
    /// <summary>
    /// Scanner registration request.
    /// </summary>
    public class RegistrationRequest
    {
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
        public string Type { get; set; }
        
        /// <summary>
        /// Gets or sets the manufacturer of the device.
        /// </summary>
        public string Manufacturer { get; set; }
    
        /// <summary>
        /// Gets or sets the model of the device.
        /// </summary>
        public string Model { get; set; }
    
        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
