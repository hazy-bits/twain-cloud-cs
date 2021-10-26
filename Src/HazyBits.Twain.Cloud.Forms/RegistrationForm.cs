using System;
using System.Diagnostics;
using System.Windows.Forms;
using HazyBits.Twain.Cloud.Registration;
using Timer = System.Threading.Timer;

namespace HazyBits.Twain.Cloud.Forms
{
    public partial class RegistrationForm : Form
    {
        private Timer pollingTimer = null;
        private readonly object objectLockTimer = new object();

        public RegistrationForm(RegistrationManager manager, RegistrationResponse registrationResponse)
        {
            InitializeComponent();

            registrationUrlLabel.Text = registrationResponse.InviteUrl;
            registrationTokenTextBox.Text = registrationResponse.RegistrationToken;

            StartPooling(manager, registrationResponse);
        }

        private void StartPooling(RegistrationManager manager, RegistrationResponse registrationResponse)
        {
            int pollingCounter = 0;
            pollingTimer = new Timer(state =>
            {
                pollingCounter++;
                var pollResult = manager.Poll(registrationResponse.PollingUrl).Result;

                if (pollResult.Success)
                {
                    StopPooling();

                    PollResponse = pollResult;
                    ShowSuccessResult();
                }
                else if (pollingCounter > 120) // 10 minutes
                {
                    StopPooling();

                    ShowFailureResult();
                }
            }, null, 0, 5000);
        }

        private void StopPooling()
        {
            lock (objectLockTimer)
            {
                if (pollingTimer != null)
                {
                    pollingTimer.Dispose();
                    pollingTimer = null;
                }
            }
        }

        public new void Dispose()
        {
            StopPooling();

            base.Dispose();
        }

        public PollResponse PollResponse { get; set; }

        private void ShowFailureResult()
        {
            if (this.InvokeRequired)
            {
                Action action = ShowFailureResult;
                this.Invoke(action);
            }
            else
            {
                progressPictureBox.Image = Properties.Resources.Error;
                statusLabel.Text = "Registration failed. Please try again.";
            }
        }

        private void ShowSuccessResult()
        {
            if (this.InvokeRequired)
            {
                Action action = ShowSuccessResult;
                this.Invoke(action);
            }
            else
            {
                progressPictureBox.Image = Properties.Resources.Information;
                statusLabel.Text = "Success!";
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((LinkLabel)sender).Text);
        }
    }
}
