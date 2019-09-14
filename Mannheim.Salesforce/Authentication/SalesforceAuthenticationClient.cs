using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceAuthenticationClient : IDisposable
    {
        private readonly SalesforceOAuthConfiguration oauthConfiguration;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor when manually creating an instance
        /// </summary>
        public SalesforceAuthenticationClient(SalesforceOAuthConfiguration configuration, Uri loginServerUri, HttpClient httpClient, ILogger<SalesforceAuthenticationClient> logger)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = loginServerUri;
            this.oauthConfiguration = configuration;
            this.logger = logger;
        }

        private async Task<T> PostToTokenEndpoint<T>(Dictionary<string, string> payload, [CallerMemberName]string caller = null)
        {
            var tokenResponse = await this.httpClient.PostAsync(this.TokenEndpoint, new FormUrlEncodedContent(payload));

            if (!tokenResponse.IsSuccessStatusCode)
            {
                throw new Exception($"${caller} failed: {tokenResponse.StatusCode} {tokenResponse.ReasonPhrase} {await tokenResponse.Content.ReadAsStringAsync()}");
            }

            var serializedToken = await tokenResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(serializedToken);
        }

        public Task<SalesforceOAuthToken> ExchangeDeviceCodeForTokenAsync(string code)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "code", code },
                { "client_id", this.oauthConfiguration.ConsumerId },
                { "grant_type", "device" }
            });
        }

        public Uri TokenEndpoint => new Uri($"{this.httpClient.BaseAddress}/services/oauth2/token");

        public void Dispose()
        {
            this.httpClient?.Dispose();
        }

        public Task<SalesforceOAuthToken> ExchangeCodeForTokenAsync(string code)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "code", code },
                { "client_id", this.oauthConfiguration.ConsumerId },
                { "client_secret", this.oauthConfiguration.ConsumerSecret },
                { "redirect_uri", this.oauthConfiguration.RedirectUrl },
                { "grant_type", "authorization_code" }
            });
        }

        public Task<SalesforceOAuthToken> ExchangeUserPasswordForTokenAsync(string user, string password, string apiToken)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "client_id", this.oauthConfiguration.ConsumerId },
                { "client_secret", this.oauthConfiguration.ConsumerSecret },
                { "username", user },
                { "password", password + apiToken},
                { "grant_type", "password" }
            });
        }

        public Task<SalesforceOAuthToken> ExchangeRefreshTokenForTokenAsync(string refreshToken)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "refresh_token", refreshToken },
                { "client_id", this.oauthConfiguration.ConsumerId },
                { "client_secret", this.oauthConfiguration.ConsumerSecret },
                { "grant_type", "refresh_token" }
            });
        }

        public Task<DeviceFlowToken> StartDeviceFlowAsync()
        {
            return this.PostToTokenEndpoint<DeviceFlowToken>(new Dictionary<string, string> {
                { "client_id", this.oauthConfiguration.ConsumerId },
                { "response_type", "device_code" },
                { "api", "api refresh_token" },
            });
        }

        public Uri GetAuthenticationUrl(string state = null)
        {
            return this.oauthConfiguration.GetAuthenticationUrl(this.httpClient.BaseAddress, state);
        }
    }
}
