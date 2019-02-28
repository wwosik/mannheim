using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceAuthenticationClient : IDisposable
    {
        private readonly SalesforceOAuthConfiguration salesforceOAuthConfig;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public SalesforceAuthenticationClient(IHttpClientFactory httpClientFactory, IOptions<SalesforceAuthenticationClientOptions> options, ILogger<SalesforceAuthenticationClient> logger)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = options.Value.LoginSystemUri;
            this.salesforceOAuthConfig = options.Value.SalesforceOAuthConfig;
            this.logger = logger;
        }

        public SalesforceAuthenticationClient(HttpClient httpClient, SalesforceAuthenticationClientOptions options, ILogger logger)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = options.LoginSystemUri;
            this.salesforceOAuthConfig = options.SalesforceOAuthConfig;
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
            return JsonConvert.DeserializeObject<T>(serializedToken);
        }

        public Task<SalesforceOAuthToken> ExchangeDeviceCodeForTokenAsync(string code)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "code", code },
                { "client_id", this.salesforceOAuthConfig.ConsumerId },
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
                { "client_id", this.salesforceOAuthConfig.ConsumerId },
                { "client_secret", this.salesforceOAuthConfig.ConsumerSecret },
                { "redirect_uri", this.salesforceOAuthConfig.RedirectUrl },
                { "grant_type", "authorization_code" }
            });
        }

        public Task<SalesforceOAuthToken> ExchangeUserPasswordForTokenAsync(string user, string password, string apiToken)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "client_id", this.salesforceOAuthConfig.ConsumerId },
                { "client_secret", this.salesforceOAuthConfig.ConsumerSecret },
                { "username", user },
                { "password", password + apiToken},
                { "grant_type", "password" }
            });
        }

        public Task<SalesforceOAuthToken> ExchangeRefreshTokenForTokenAsync(string refreshToken)
        {
            return this.PostToTokenEndpoint<SalesforceOAuthToken>(new Dictionary<string, string> {
                { "refresh_token", refreshToken },
                { "client_id", this.salesforceOAuthConfig.ConsumerId },
                { "client_secret", this.salesforceOAuthConfig.ConsumerSecret },
                { "grant_type", "refresh_token" }
            });
        }

        public Task<DeviceFlowToken> StartDeviceFlowAsync()
        {
            return this.PostToTokenEndpoint<DeviceFlowToken>(new Dictionary<string, string> {
                { "client_id", this.salesforceOAuthConfig.ConsumerId },
                { "response_type", "device_code" },
                { "api", "api refresh_token" },
            });
        }

        public Uri GetAuthenticationUrl(string state = null)
        {
            return this.salesforceOAuthConfig.GetAuthenticationUrl(this.httpClient.BaseAddress, state);
        }
    }
}
