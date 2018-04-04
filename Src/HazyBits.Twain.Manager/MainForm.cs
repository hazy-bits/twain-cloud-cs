using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HazyBits.Twain.Cloud.Application;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Device;
using HazyBits.Twain.Cloud.Registration;

namespace HazyBits.Twain.Manager
{
    public partial class MainForm : Form
    {
        private ApplicationManager _applicationManager;
        private static string SettingsFile = @"C:\Dev\scanners.csv";

        public MainForm()
        {
            InitializeComponent();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();
            about.ShowDialog();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var loginPopup = new FacebookLoginForm();
            loginPopup.Authorized += async (_, args) =>
            {
                var client = new TwainCloudClient(Constants.ApiRoot, args.Tokens);
                _applicationManager = new ApplicationManager(client);
                await LoadScanners();
                loginPopup.Close();
            };

            loginPopup.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void registerButton_Click(object sender, EventArgs e)
        {
            var client = new TwainCloudClient(Constants.ApiRoot);
            var registrationManager = new RegistrationManager(client);

            var result = await registrationManager.Register(new ScannerInformation
            {
                Name = "Virtual Scanner",
                Description = "Home Virtual Scanner",
                Manufacturer = "Home & Co"
            });

            var registrationDialog = new RegistrationForm(registrationManager, result);
            registrationDialog.ShowDialog();

            var pollResult = registrationDialog.PollResponse;
            if (pollResult != null)
            {
                SaveScannerRegistration(result.ScannerId, pollResult.AuthorizationToken, pollResult.RefreshToken);
            }
        }

        private static void SaveScannerRegistration(string scannerId, string authToken, string refreshToken)
        {
            File.AppendAllLines(SettingsFile, new [] {$"{scannerId};{authToken};{refreshToken}"});
        }

        private async Task LoadScanners()
        {
            var scanners = await _applicationManager.GetScanners();
            foreach (var scanner in scanners)
            {
                var info = await _applicationManager.GetScannerInfo(scanner.Id);
            }
        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            var scanners = File.ReadAllLines(SettingsFile);
            var scannerInfo = scanners.Last().Split(';');
            var scannerId = scannerInfo[0];

            var tokens = new TwainCloudTokens(scannerInfo[1], scannerInfo[2]);
            var client = new TwainCloudClient(Constants.ApiRoot, tokens);

            client.TokensRefreshed += (o, args) =>
                {
                    SaveScannerRegistration(scannerId, args.Tokens.AuthorizationToken, args.Tokens.RefreshToken);
                };

            var deviceSession = new DeviceSession(client);    
            deviceSession.Received += (o, message) => { Debug.WriteLine("message received: " + message); };

            await deviceSession.Connect(scannerId);
            await deviceSession.Send("test message to the cloud");
        }
    }   
}
