using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceClientProvider
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILoggerFactory loggerFactory;

        public ISalesforceConfigStore ConfigStore { get; }

        public SalesforceClientProvider(IHttpClientFactory httpClientFactory, ISalesforceConfigStore configStore, ILoggerFactory loggerFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.ConfigStore = configStore;
            this.loggerFactory = loggerFactory;
        }

        public Task<SalesforceClient> GetClientAsync(string name = "__default", bool cache = true)
        {
            if (cache && this.clientCache.TryGetValue(name, out var cachedClient))
            {
                return Task.FromResult(cachedClient);
            }

            return this.CreateClientAsync(name, cache);
        }

        private readonly Dictionary<string, SalesforceClient> clientCache = new Dictionary<string, SalesforceClient>();

        public async Task<SalesforceClient> CreateClientAsync(string name = "__default", bool cache = true)
        {
            var oauthConfig = await this.ConfigStore.GetSalesforceOAuthConfigurationAsync();
            var savedToken = await this.ConfigStore.GetSalesforceTokenAsync(name);
            var logger = this.loggerFactory.CreateLogger<SalesforceAuthenticationClient>();
            using (var authClient = await this.CreateSalesforceAuthenticationClientAsync(new Uri(savedToken.InstanceUrl)))
            {
                var newToken = await authClient.ExchangeRefreshTokenForTokenAsync(savedToken.RefreshToken);

                var client = new SalesforceClient(this.httpClientFactory.CreateClient(), newToken, logger);
                this.clientCache[name] = client;
                return client;
            }
        }

        public async Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null)
        {
            var oauthConfig = await this.ConfigStore.GetSalesforceOAuthConfigurationAsync();
            return oauthConfig.GetAuthenticationUrl(loginSystemUri, state);
        }

        public async Task AddClientForToken(Uri loginSystemUri, string code, string name = null)
        {
            using (var authClient = await this.CreateSalesforceAuthenticationClientAsync(loginSystemUri))
            {
                var token = await authClient.ExchangeCodeForTokenAsync(code);
                await this.ConfigStore.SaveSalesforceTokenAsync(token, name);
            }
        }

        public async Task<SalesforceAuthenticationClient> CreateSalesforceAuthenticationClientAsync(Uri loginSystemUri)
        {
            var config = await this.ConfigStore.GetSalesforceOAuthConfigurationAsync();
            var httpClient = this.httpClientFactory.CreateClient();
            var options = new SalesforceAuthenticationClientOptions
            {
                LoginSystemUri = loginSystemUri,
                SalesforceOAuthConfig = config
            };
            var logger = this.loggerFactory.CreateLogger<SalesforceAuthenticationClient>();
            return new SalesforceAuthenticationClient(httpClient, options, logger);
        }
    }
}
