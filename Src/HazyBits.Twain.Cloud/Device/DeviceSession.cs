using System;
using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Events;

namespace HazyBits.Twain.Cloud.Device
{
    /// <summary>
    /// Class that handles connection to TWAIN Cloud infrastructure from device side of things.
    /// </summary>
    public class DeviceSession
    {
        #region Private Fields

        private readonly TwainCloudClient _client;
        private MqttClient _mqttClient;
        private string _cloudTopicName;

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSession"/> class.
        /// </summary>
        /// <param name="client">Initialized TWAIN Cloud client.</param>
        public DeviceSession(TwainCloudClient client)
        {
            _client = client;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when asynchronious message from TWAIN Cloud is received.
        /// </summary>
        public event EventHandler<string> Received;

        #endregion

        #region Public Methods

        /// <summary>
        /// Connects specified scanner to TWAIN Cloud infrastructure.
        /// </summary>
        /// <param name="scannerId">The scanner identifier.</param>
        /// <returns></returns>
        public async Task Connect(string scannerId)
        {
            var scannerInfo = await _client.Get<ScannerStatusResponse>($"scanners/{scannerId}");

            _cloudTopicName = scannerInfo.CloudTopic;
            await ConnectToMqttBroker(scannerInfo.Url, scannerInfo.DeviceTopic);
        }

        /// <summary>
        /// Disconnects the session from TWAIN Cloud infrastructure.
        /// </summary>
        public void Disconnect()
        {
            _mqttClient.Dispose();
            _mqttClient = null;
        }

        /// <summary>
        /// Sends the specified message to the TWAIN Cloud.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the session is in disconnected state.</exception>
        public async Task Send(string message)
        {
            if (_mqttClient == null)
                throw new InvalidOperationException();

            await _mqttClient.Publish(_cloudTopicName, message);
        }

        /// <summary>
        /// Uploads the specified data to the TWAIN Cloud.
        /// </summary>
        /// <param name="data">The data to upload.</param>
        /// <returns>Unique ID of the object stored in the cloud.</returns>
        public string Upload(byte[] data)
        {
            // TODO: TBD
            return string.Empty;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Received" /> event.
        /// </summary>
        /// <param name="message">The received message.</param>
        protected virtual void OnReceived(string message)
        {
            Received?.Invoke(this, message);
        }

        #endregion

        #region Private Methods

        private async Task ConnectToMqttBroker(string mqttUrl, string topic)
        {
            _mqttClient = new MqttClient(mqttUrl);
            _mqttClient.MessageReceived += (_, message) => { OnReceived(message.Message); };

            await _mqttClient.Connect();
            await _mqttClient.Subscribe(topic);
        }

        #endregion
    }
}
