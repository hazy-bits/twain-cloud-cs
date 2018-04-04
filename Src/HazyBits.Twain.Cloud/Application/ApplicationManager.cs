using System.Collections.Generic;
using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Registration;

namespace HazyBits.Twain.Cloud.Application
{
    /// <summary>
    /// Class that handles connection to TWAIN Cloud infrastructure from application side.
    /// </summary>
    public class ApplicationManager
    {
        #region Private Fields

        private readonly TwainCloudClient _client;

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationManager"/> class.
        /// </summary>
        /// <param name="client">Initialized TWAIN Cloud client.</param>
        public ApplicationManager(TwainCloudClient client)
        {
            _client = client;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of scanners registered in the TWAIN Cloud.
        /// </summary>
        /// <returns>List of scanners registered for the current user.</returns>
        public async Task<IEnumerable<ScannerInformation>> GetScanners()
        {
            return await _client.Get<List<ScannerInformation>>("scanners");
        }

        /// <summary>
        /// Gets the scanner information for specified scanner ID.
        /// </summary>
        /// <param name="scannerId">The scanner identifier.</param>
        /// <returns>Scanner information for specified scanner ID.</returns>
        public async Task<ScannerInformation> GetScannerInfo(string scannerId)
        {
            return await _client.Get<ScannerInformation>($"scanners/{scannerId}/privet/infoex");
        }

        /*
        public async Task<> SendCommand(string scannerId, string command)
        {
            return await _client.Post<>($"scanners/{scannerId}/privet/twaindirect/session");
        }
        */

        #endregion
    }
}
