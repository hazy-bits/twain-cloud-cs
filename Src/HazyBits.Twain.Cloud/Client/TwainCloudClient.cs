using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HazyBits.Twain.Cloud.Client
{
    /// <summary>
    /// Low level TWAIN Cloud client that handles connection to the infrastructure.
    /// </summary>
    public class TwainCloudClient
    {
        #region Static Fields 

        private static readonly JsonSerializerSettings JsonSettings;
        private static readonly JsonMediaTypeFormatter DefaultFormatter;

        #endregion

        #region Private Fields

        private TwainCloudTokens _tokens;
        private readonly string _rootUrl;
        private readonly HttpClient _client = new HttpClient();

        #endregion

        #region Ctors

        /// <summary>
        /// Initializes the <see cref="TwainCloudClient"/> class.
        /// </summary>
        static TwainCloudClient()
        {            
            JsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            DefaultFormatter = new JsonMediaTypeFormatter { SerializerSettings = JsonSettings };
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TwainCloudClient"/> class.
        /// </summary>
        /// <param name="rootUrl">The root TWAIN Cloud API URL.</param>
        /// <param name="tokens">TWAIN Cloud access tokens.</param>
        public TwainCloudClient(string rootUrl, TwainCloudTokens tokens = null)
        {
            _rootUrl = rootUrl;
            UpdateTokens(tokens);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when TWAIN Cloud access tokens are refreshed.
        /// </summary>
        public event EventHandler<TokensRefreshedEventArgs> TokensRefreshed;

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends POST request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="endpoint">Request endpoint (either absolute or relative to the root URL).</param>
        /// <param name="body">Payload for the request to send.</param>
        /// <returns>Deserialied payload of the response.</returns>
        public async Task<TResult> Post<TResult>(string endpoint, object body)
        {
            return await ExecuteRequest<TResult>(() => _client.PostAsync(GetEndpointUrl(endpoint), body, DefaultFormatter));          
        }

        /// <summary>
        /// Sends GET request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="endpoint">Request endpoint (either absolute or relative to the root URL).</param>
        /// <returns>Deserialied payload of the response.</returns>
        public async Task<TResult> Get<TResult>(string endpoint)
        {
            return await ExecuteRequest<TResult>(() => _client.GetAsync(GetEndpointUrl(endpoint)));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="TokensRefreshed" /> event.
        /// </summary>
        /// <param name="args">The <see cref="TokensRefreshedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTokensRefreshed(TokensRefreshedEventArgs args)
        {
            TokensRefreshed?.Invoke(this, args);
        }

        #endregion

        #region Private Methods

        private string GetEndpointUrl(string endpoint)
        {
            // TODO: kind of ugly, but should work
            return Uri.IsWellFormedUriString(endpoint, UriKind.Absolute) ? endpoint : $"{_rootUrl}/{endpoint}";
        }

        private static async Task<TResult> DeserializeObject<TResult>(HttpResponseMessage httpResponse)
        {
            var response = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(response, JsonSettings);
        }

        private async Task<TResult> ExecuteRequest<TResult>(Func<Task<HttpResponseMessage>> request)
        {
            var response = await request();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshResponse = await _client.GetAsync(GetEndpointUrl($"authentication/refresh/{_tokens?.RefreshToken}"));
                var tokens = await DeserializeObject<TwainCloudTokens>(refreshResponse);
                UpdateTokens(tokens);
                response = await request();
            }

            return await DeserializeObject<TResult>(response);
        }

        private void UpdateTokens(TwainCloudTokens tokens)
        {
            _tokens = tokens;

            if (_tokens?.AuthorizationToken != null)
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _tokens.AuthorizationToken);
            }

            OnTokensRefreshed(new TokensRefreshedEventArgs { Tokens = _tokens });
        }

        #endregion
    }
}
