using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Mannheim.Salesforce.Client.RestApi;
using Mannheim.Salesforce.ConnectionManagement;
using Mannheim.Storage;
using Mannheim.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceClientFactory : ISalesforceClientFactory
    {
        private readonly ILogger<SalesforceClientFactory> logger;
        private readonly IObjectStore safeStore;
        private readonly IServiceProvider serviceProvider;
        private readonly SalesforceClientFactoryOptions options;

        public SalesforceClientFactory(ILogger<SalesforceClientFactory> logger, IObjectStore safeStore, IServiceProvider serviceProvider, IOptions<SalesforceClientFactoryOptions> options)
        {
            this.logger = logger;
            this.safeStore = safeStore ?? throw new ArgumentNullException("safeStore");
            this.serviceProvider = serviceProvider;
            this.options = options?.Value ?? new SalesforceClientFactoryOptions();
        }

        public Task<SalesforceOAuthToken> GetTokenAsync(string name)
        {
            this.logger.LogTrace($"Getting SalesforceOAuthToken {name}");
            return safeStore.ReadAsync<SalesforceOAuthToken>(this.options.SalesforceOAuthTokenStoreCategoryName, name);
        }

        public async Task<SalesforceOAuthConfiguration> GetOAuthConfigurationAsync(string oauthConfigName = "__default")
        {
            var task = this.safeStore.ReadAsync<SalesforceOAuthConfiguration>(this.options.SalesforceOAuthConfigurationCategoryName, oauthConfigName);
            var configuration = await task;
            if (configuration == null) throw new InvalidOperationException("Salesforce Oauth not configured!");
            return configuration;
        }

        public async Task SaveOAuthConfigurationAsync(SalesforceOAuthConfiguration configuration, string oauthConfigName = "__default")
        {
            await this.safeStore.WriteAsync(this.options.SalesforceOAuthConfigurationCategoryName, oauthConfigName, configuration);
        }

        public async Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null)
        {
            var oauthConfig = await this.GetOAuthConfigurationAsync();
            return oauthConfig.GetAuthenticationUrl(loginSystemUri, state);
        }

        public async Task<SalesforceAuthenticationClient> CreateAuthenticationClientAsync(Uri uri, string oauthConfigName = "__default")
        {
            var oauthOptions = await GetOAuthConfigurationAsync(oauthConfigName);
            return new SalesforceAuthenticationClient(
                configuration: oauthOptions,
                loginServerUri: uri,
                httpClient: this.serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(),
                logger: this.serviceProvider.GetRequiredService<ILogger<SalesforceAuthenticationClient>>()
                );
        }

        public async Task<SalesforceClient> CreateClientAsync(string name, ApiVersion apiVersion = null, string oauthConfigName = "__default")
        {
            var savedToken = await this.GetTokenAsync(name);
            if (savedToken == null)
            {
                throw new KeyNotFoundException($"Connection {name} unknown");
            }

            using (var authClient = await CreateAuthenticationClientAsync(new Uri(savedToken.InstanceUrl, UriKind.Absolute), oauthConfigName))
            {

                this.logger.LogDebug($"Authenticating {name}...");
                var newToken = await authClient.ExchangeRefreshTokenForTokenAsync(savedToken.RefreshToken);
                this.logger.LogDebug($"Authenticated {newToken.IdTokenUserInfo?.Name}...");


                var salesforceClient = new SalesforceClient(
                    httpClient: this.serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(),
                    salesforceToken: newToken,
                    logger: this.serviceProvider.GetRequiredService<ILogger<SalesforceClient>>());

                if (apiVersion == null)
                {
                    this.logger.LogTrace("Establishing newest API version");
                    await salesforceClient.SetToUseNewestApiVersionAsync();
                    this.logger.LogTrace($"API version: {salesforceClient.ApiVersion}");
                }

                return salesforceClient;
            }
        }

        public Task SaveTokenAsync(string name, SalesforceOAuthToken token)
        {
            return this.safeStore.WriteAsync(this.options.SalesforceOAuthTokenStoreCategoryName, name, token);
        }

        public Task<ICollection<(string, SalesforceOAuthToken)>> EnumerateTokensAsync()
        {
            return this.safeStore.EnumerateCategoryAsync<SalesforceOAuthToken>(this.options.SalesforceOAuthTokenStoreCategoryName);
        }
    }
}
