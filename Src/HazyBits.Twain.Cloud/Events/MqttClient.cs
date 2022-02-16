using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Telemetry;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Diagnostics;

namespace HazyBits.Twain.Cloud.Events
{
    /// <summary>
    /// TWAIN Cloud MQTT client.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class MqttClient : IDisposable
    {
        #region Private Fields

        private static readonly Logger Logger = Logger.GetLogger<MqttClient>();
        private static readonly Encoding DefaultMessageEncoding = Encoding.UTF8;

        private readonly IMqttClient _client;
        private readonly IMqttClientOptions _options;
        private static readonly MqttNetLogger _logger;
        private List<string> _topicsSubscription = new List<string>();

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes the <see cref="MqttClient"/> class.
        /// </summary>
        static MqttClient()
        {
            #if DEBUG
            _logger = new MqttNetLogger();

            // Write all trace messages to the console window.
            _logger.LogMessagePublished += (s, e) =>
            {
                var trace = $">> [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}";
                if (e.LogMessage.Exception != null)
                    trace += Environment.NewLine + e.LogMessage.Exception.ToString();

                Debug.WriteLine(trace);
                Logger.LogDebug(trace);
            };

            #endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttClient"/> class.
        /// </summary>
        /// <param name="mqttUrl">The MQTT broker URL.</param>
        public MqttClient(string mqttUrl)
        {
            // Create a new MQTT client.
            var factory = new MqttFactory(_logger);
            _client = factory.CreateMqttClient();

            // Use WebSocket connection.
            _options = new MqttClientOptionsBuilder()
                .WithWebSocketServer(mqttUrl)
                .WithTls()
                .WithClientId("twain-direct-proxy-" + Guid.NewGuid()) // TODO: define this constant somewhere
                .Build();
            _client.UseApplicationMessageReceivedHandler(MqttMessagePublishReceived);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when new message from MQTT broker is received.
        /// </summary>
        public event EventHandler<MqttMessage> MessageReceived;

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }

        /// <summary>
        /// Connects to MQTT broker.
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            _client.UseDisconnectedHandler(async e =>
            {
                Logger.LogDebug("Disconnected from server");

                // TODO: implement exponential backoff instead.
                await Task.Delay(TimeSpan.FromSeconds(2));

                try
                {
                    await ConnectMqttBroker();

                    // The topic subscriptions are made again when the mqtt server reconnection is stablished
                    // (for example, it has to be done when the mqtt server is restarted)
                    if (_topicsSubscription.Count > 0)
                    {
                        foreach(string topic in _topicsSubscription)
                        {
                            await Subscribe(topic);
                        }
                    }
                }
                catch
                {
                    Logger.LogDebug("Reconnection failed");
                }
            });

            await ConnectMqttBroker();
        }

        /// <summary>
        /// Subscribes to the specified topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns></returns>
        public async Task Subscribe(string topic)
        {
            using (Logger.StartActivity($"Subscribing to topic: {topic}"))
            {
                // '#' is the wildcard to subscribe to anything under the 'root' topic
                // the QOS level here - I only partially understand why it has to be this level - it didn't seem to work at anything else.
                await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                // The topic subscription is made again when the mqtt server reconnection is stablished
                _topicsSubscription.Add(topic);
            }
        }

        /// <summary>
        /// Publishes a message to the specified topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public async Task Publish(string topic, string message)
        {
            using (Logger.StartActivity($"Publishing a message to topic: {topic}"))
            {
                Logger.LogDebug(message);
                await _client.PublishAsync(new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = DefaultMessageEncoding.GetBytes(message)
                });
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="MessageReceived" /> event.
        /// </summary>
        /// <param name="message">Message payload.</param>
        protected virtual void OnMessageReceived(MqttMessage message)
        {
            using (Logger.StartActivity($"Receiving message from topic: {message.Topic}"))
            {
                Logger.LogDebug(message.Message);
                MessageReceived?.Invoke(this, message);
            }
        }

        #endregion

        #region Private Methods

        private async Task ConnectMqttBroker()
        {
            using (Logger.StartActivity("Connecting to broker"))
            {
                await _client.ConnectAsync(_options);
            }
        }

        private async Task MqttMessagePublishReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            OnMessageReceived(new MqttMessage { Topic = e.ApplicationMessage.Topic, Message = DefaultMessageEncoding.GetString(e.ApplicationMessage.Payload) });

            await Task.FromResult(0);
        }

        #endregion
    }
}
