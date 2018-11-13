using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HazyBits.Twain.Cloud.Application;
using HazyBits.Twain.Cloud.Client;
using HazyBits.Twain.Cloud.Device;
using HazyBits.Twain.Cloud.Forms;
using HazyBits.Twain.Cloud.Registration;
using HazyBits.Twain.Manager.Storage;

namespace HazyBits.Twain.Manager
{
    public partial class MainForm : Form
    {
        private ApplicationManager _applicationManager;

        public MainForm()
        {
            InitializeComponent();

            // load registered devices
            LoadRegisteredScanners();
        }

        private void LoadRegisteredScanners()
        {
            if (this.InvokeRequired)
            {
                Action action = LoadRegisteredScanners;
                Invoke(action, new object[] { });
            }
            else
            {
                var scanners = GetRegisteredScanners();
                registeredDevicesComboBox.Items.Clear();
                registeredDevicesComboBox.Items.AddRange(scanners.ToArray());
            }
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
            var loginPopup = new FacebookLoginForm(Constants.LoginUrl);
            loginPopup.Authorized += async (_, args) =>
            {
                loginPopup.Close();
                var client = new TwainCloudClient(Constants.ApiRoot, args.Tokens);
                _applicationManager = new ApplicationManager(client);
                await LoadScanners();
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

            var scannerInfo = new RegistrationRequest
            {
                Name = "Virtual Scanner",
                Description = "Home Virtual Scanner",
                Manufacturer = "Home & Co"
            };

            var result = await registrationManager.Register(scannerInfo);
            var registrationDialog = new RegistrationForm(registrationManager, result);
            registrationDialog.ShowDialog();

            var pollResult = registrationDialog.PollResponse;
            if (pollResult != null)
            {
                SaveScannerRegistration(new CloudScanner
                {
                    Id = result.ScannerId,
                    Name = scannerInfo.Name,
                    AuthorizationToken = pollResult.AuthorizationToken,
                    RefreshToken = pollResult.RefreshToken
                });

                LoadRegisteredScanners();
            }
        }

        private static void SaveScannerRegistration(CloudScanner scanner)
        {
            using (var context = new CloudContext())
            {
                context.Scanners.AddOrUpdate(scanner);
                context.SaveChanges();
            }
        }

        private static List<CloudScanner> GetRegisteredScanners()
        {
            using (var context = new CloudContext())
            {
                return context.Scanners.ToList();
            }
        }

        private async Task LoadScanners()
        {
            _applicationManager.Received += (sender, s) => { MessageBox.Show(s); };
            await _applicationManager.Connect();

            var scanners = await _applicationManager.GetScanners();
            foreach (var scanner in scanners)
            {

                var listItem = CreateScannerListItem(await _applicationManager.GetScannerInfo(scanner.Id));
                cloudScannersListView.Items.Add(listItem);
            }
        }

        private static ListViewItem CreateScannerListItem(ScannerInformation scanner)
        {
            string[] row = {scanner.Id, scanner.Name, scanner.Manufacturer};
            return new ListViewItem(row);
        }

        private async void connectButton_Click_1(object sender, EventArgs e)
        {
            var scanner = registeredDevicesComboBox.SelectedItem as CloudScanner;

            var tokens = new TwainCloudTokens(scanner.AuthorizationToken, scanner.RefreshToken);
            var client = new TwainCloudClient(Constants.ApiRoot, tokens);

            client.TokensRefreshed += (o, args) =>
            {
                scanner.AuthorizationToken = args.Tokens.AuthorizationToken;
                scanner.RefreshToken = args.Tokens.RefreshToken;
                SaveScannerRegistration(scanner);
            };

            var deviceSession = new DeviceSession(client, scanner.Id);    
            deviceSession.Received += async (o, message) =>
            {
                Debug.WriteLine("cloud message received: " + message);
                await deviceSession.Send("message received");
            };

            await deviceSession.Connect(scanner.Id);
        }

        private async void createSessionButton_Click(object sender, EventArgs e)
        {
            var command =
                "{\r\n    \"kind\": \"twainlocalscanner\",\r\n    \"commandId\": \"{{$guid}}\",\r\n    \"method\": \"createSession\"\r\n}";

            var selectedScanner = cloudScannersListView.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (selectedScanner != null)
            {
                await _applicationManager.SendCommand(selectedScanner.Text, command);
            }
        }

        private void configureButton_Click(object sender, EventArgs e)
        {

        }

        private async void registeredDevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var scanner = registeredDevicesComboBox.SelectedItem as CloudScanner;

            //var info = await _applicationManager.GetScannerInfo(scanner.Id);
            //Debug.WriteLine(info.Id);
        }
    }   
}
