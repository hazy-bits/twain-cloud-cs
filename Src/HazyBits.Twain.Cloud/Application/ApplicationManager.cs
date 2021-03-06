﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Registration;
using HazyBits.Twain.Cloud.Telemetry;

namespace HazyBits.Twain.Cloud.Application
{
    /// <summary>
    /// Class that handles connection to TWAIN Cloud infrastructure from application side.
    /// </summary>
    public class ApplicationManager: EventBrokerClient
    {
        #region Private Fields

        private static Logger Logger = Logger.GetLogger<ApplicationManager>();

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

        public async Task Connect()
        {
            using (Logger.StartActivity("Connecting to cloud infrastructure"))
            {
                var userInfo = await GetUserInformation();

                await base.Connect(userInfo.EventBroker.Url);
                await base.Subscribe(userInfo.EventBroker.Topic);
            }
        }

        public async Task<UserInformation> GetUserInformation()
        {
            return await _client.Get<UserInformation>("user");
        }

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

        /// <summary>
        /// Sends the command to the specified scanner.
        /// </summary>
        /// <param name="scannerId">The scanner identifier.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task SendCommand(string scannerId, string command)
        {
            await _client.Post<object>($"scanners/{scannerId}/privet/twaindirect/session", command);
        }

        /// <summary>
        /// Downloads the block.
        /// </summary>
        /// <param name="scannerId">The scanner identifier.</param>
        /// <param name="blockUrl">The block URL.</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadBlock(string blockUrl)
        {
            var blockBytes = await _client.Get<byte[]>(blockUrl);
            return blockBytes;
        }

        #endregion
    }
}
