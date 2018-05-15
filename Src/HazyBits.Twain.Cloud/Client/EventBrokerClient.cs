using System;
using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Events;

namespace HazyBits.Twain.Cloud.Client
{
    public abstract class EventBrokerClient: IDisposable
    {
        private MqttClient _mqttClient;

        #region Events

        /// <summary>
        /// Occurs when asynchronious message from TWAIN Cloud is received.
        /// </summary>
        public event EventHandler<string> Received;

        #endregion

        public async Task Connect(string url)
        {
            _mqttClient = new MqttClient(url);
            _mqttClient.MessageReceived += (_, message) => { OnReceived(message.Message); };

            await _mqttClient.Connect();
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }

        /// <summary>
        /// Sends the specified message to the TWAIN Cloud.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the session is in disconnected state.</exception>
        public async Task Send(string topic, string message)
        {
            ValidateState();
            await _mqttClient.Publish(topic, message);
        }

        public async Task Subscribe(string topic)
        {
            ValidateState();
            await _mqttClient.Subscribe(topic);
        }



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

        private void ValidateState()
        {
            if (_mqttClient == null)
                throw new InvalidOperationException("Broker is in disconnected state. Call Connect first.");
        }
    }
}
