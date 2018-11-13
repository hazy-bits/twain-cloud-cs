using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Telemetry;

namespace HazyBits.Twain.Cloud.Registration
{
    /// <summary>
    /// Class that handles device registration with TWAIN Cloud infrastructure.
    /// </summary>
    public class RegistrationManager
    {
        #region Private Fields

        private static Logger Logger = Logger.GetLogger<RegistrationManager>();

        private readonly TwainCloudClient _client;

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationManager"/> class.
        /// </summary>
        /// <param name="client">Initialized TWAIN Cloud client.</param>
        public RegistrationManager(TwainCloudClient client)
        {
            _client = client;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initiates registration sequence for specified device.
        /// </summary>
        /// <param name="request">The device information to register.</param>
        /// <returns>Registration response.</returns>
        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            using (Logger.StartActivity($"Registering scanner: {request.Name} ({request.Description})"))
            {
                return await _client.Post<RegistrationResponse>("register", request);
            }
        }

        /// <summary>
        /// Polls TWAIN Cloud infrastucture for registration completion.
        /// </summary>
        /// <param name="pollUrl">The poll URL.</param>
        /// <returns>Polling reponse.</returns>
        public async Task<PollResponse> Poll(string pollUrl)
        {
            using (Logger.StartActivity($"Polling scanner registration: {pollUrl}"))
            {
                return await _client.Get<PollResponse>(pollUrl);
            }
        }

        #endregion
    }
}