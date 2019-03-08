using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Microsoft.Extensions.Logging;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceClientProvider
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger logger;

        public ISalesforceConfigStore ConfigStore { get; }

        public SalesforceClientProvider(IHttpClientFactory httpClientFactory, ISalesforceConfigStore configStore, ILoggerFactory loggerFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.ConfigStore = configStore;
            this.loggerFactory = loggerFactory;
            this.logger = this.loggerFactory.CreateLogger(this.GetType().Name);
        }

        /// <summary>
        /// Provides a SalesforceClient that is already authenticated with Salesforce. If SalesforceClient already existed it is returned from the store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public Task<SalesforceClient> GetClientAsync(string name = "__default")
        {
            if (this.clientCache.TryGetValue(name, out var cachedClient))
            {
                return Task.FromResult(cachedClient);
            }

            return this.CreateClientAsync(name, true);
        }

        private readonly Dictionary<string, SalesforceClient> clientCache = new Dictionary<string, SalesforceClient>();

        public async Task<SalesforceClient> CreateClientAsync(string name = "__default", bool cache = true)
        {
            var oauthConfig = await this.ConfigStore.GetSalesforceOAuthConfigurationAsync();
            var savedToken = await this.ConfigStore.GetSalesforceTokenAsync(name);
            var authLogger = this.loggerFactory.CreateLogger<SalesforceAuthenticationClient>();
            using (var authClient = await this.CreateSalesforceAuthenticationClientAsync(new Uri(savedToken.InstanceUrl)))
            {
                this.logger.LogDebug($"Authenticating {name}...");
                var newToken = await authClient.ExchangeRefreshTokenForTokenAsync(savedToken.RefreshToken);
                this.logger.LogDebug($"Authenticated {newToken.IdTokenUserInfo?.Name}...");

                var clientLogger = this.loggerFactory.CreateLogger($"SalesforceClient [{name}]");
                var client = new SalesforceClient(this.httpClientFactory.CreateClient(), newToken, authLogger);
                if (cache)
                {
                    this.logger.LogInformation($"Added client {name} to cache");
                    this.clientCache[name] = client;
                }
                return client;
            }
        }

        public async Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null)
        {
            var oauthConfig = await this.ConfigStore.GetSalesforceOAuthConfigurationAsync();
            return oauthConfig.GetAuthenticationUrl(loginSystemUri, state);
        }

        public async Task<SalesforceOAuthToken> AddClientForCodeAsync(Uri loginSystemUri, string code, string name = null)
        {
            using (var authClient = await this.CreateSalesforceAuthenticationClientAsync(loginSystemUri))
            {
                var token = await authClient.ExchangeCodeForTokenAsync(code);
                await this.ConfigStore.SaveSalesforceTokenAsync(token, name);
                return token;
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
