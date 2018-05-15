using System;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;
using HazyBits.Twain.Cloud.Client;

namespace HazyBits.Twain.Cloud.Forms
{
    public partial class FacebookLoginForm : Form
    {
        public const string AuthorizationTokenName = "authorization_token";
        public const string RefreshTokenName = "refresh_token";

        public FacebookLoginForm(string loginUrl)
        {
            InitializeComponent();

            webBrowser.Navigate(loginUrl);
        }

        public event EventHandler<TwainCloudAuthorizedEventArgs> Authorized; 

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Debug.WriteLine(e.Url.ToString());

            var queryParams = HttpUtility.ParseQueryString(e.Url.Query);

            var authToken = queryParams[AuthorizationTokenName];
            var refreshToken = queryParams[RefreshTokenName];

            OnAuthorized(new TwainCloudAuthorizedEventArgs(new TwainCloudTokens(authToken, refreshToken)));
        }

        protected virtual void OnAuthorized(TwainCloudAuthorizedEventArgs e)
        {
            Authorized?.Invoke(this, e);
        }
    }

    public class TwainCloudAuthorizedEventArgs : EventArgs
    {
        public TwainCloudAuthorizedEventArgs(TwainCloudTokens tokens)
        {
            Tokens = tokens;
        }

        public TwainCloudTokens Tokens { get; }
    }
}
