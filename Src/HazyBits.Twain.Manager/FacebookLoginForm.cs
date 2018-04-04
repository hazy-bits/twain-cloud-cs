using System;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;
using HazyBits.Twain.Cloud.Client;

namespace HazyBits.Twain.Manager
{
    public partial class FacebookLoginForm : Form
    {
        public const string LoginUrl =
            "https://phdp8cjsmd.execute-api.us-east-1.amazonaws.com/dev/authentication/signin/facebook";

        public const string AuthorizationTokenName = "authorization_token";
        public const string RefreshTokenName = "refresh_token";

        public FacebookLoginForm()
        {
            InitializeComponent();

            webBrowser.Navigate(LoginUrl);
        }

        public event EventHandler<TwainCloudAuthorizedEventArgs> Authorized; 

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Debug.WriteLine(e.Url.ToString());

            if (e.Url.Host == "twain.hazybits.com")
            {
                var queryParams = HttpUtility.ParseQueryString(e.Url.Query);

                var authToken = queryParams[AuthorizationTokenName];
                var refreshToken = queryParams[RefreshTokenName];

                OnAuthorized(new TwainCloudAuthorizedEventArgs(new TwainCloudTokens(authToken, refreshToken)));
            }
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
