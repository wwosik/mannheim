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
            this.safeStore = safeStore;
            this.serviceProvider = serviceProvider;
            this.options = options?.Value ?? new SalesforceClientFactoryOptions();
        }

        public Task<SalesforceOAuthToken> GetTokenAsync(string name)
        {
            this.logger.LogTrace($"Getting SalesforceOAuthToken {name}");
            return safeStore.ReadAsync<SalesforceOAuthToken>(this.options.SalesforceOAuthConfigurationCategoryName, name);
        }

        public async Task<SalesforceOAuthConfiguration> GetOAuthConfigurationAsync()
        {
            var configuration = await this.safeStore.ReadAsync<SalesforceOAuthConfiguration>(this.options.SalesforceOAuthConfigurationCategoryName, this.options.SalesforceOAuthConfigurationKey);
            if (configuration == null) throw new InvalidOperationException("Salesforce Oauth not configured!");
            return configuration;
        }

        public async Task SaveOAuthConfigurationAsync(SalesforceOAuthConfiguration configuration)
        {
            await this.safeStore.WriteAsync(this.options.SalesforceOAuthConfigurationCategoryName, this.options.SalesforceOAuthConfigurationKey, configuration);
        }

        public async Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null)
        {
            var oauthConfig = await this.GetOAuthConfigurationAsync();
            return oauthConfig.GetAuthenticationUrl(loginSystemUri, state);
        }

        public async Task<SalesforceAuthenticationClient> CreateAuthenticationClientAsync(Uri uri)
        {
            var oauthOptions = await GetOAuthConfigurationAsync();
            return this.serviceProvider.Build<SalesforceAuthenticationClient>(options, uri);
        }

        public async Task<SalesforceClient> CreateClientAsync(string name, ApiVersion apiVersion = null)
        {
            var savedToken = await this.GetTokenAsync(name);
            if (savedToken == null)
            {
                throw new KeyNotFoundException($"Connection {name} unknown");
            }

            using var authClient = await CreateAuthenticationClientAsync(new Uri(savedToken.InstanceUrl, UriKind.Absolute));

            this.logger.LogDebug($"Authenticating {name}...");
            var newToken = await authClient.ExchangeRefreshTokenForTokenAsync(savedToken.RefreshToken);
            this.logger.LogDebug($"Authenticated {newToken.IdTokenUserInfo?.Name}...");

            var salesforceClient = ActivatorUtilities.CreateInstance<SalesforceClient>(this.serviceProvider, newToken);
            if (apiVersion == null)
            {
                this.logger.LogTrace("Establishing newest API version");
                await salesforceClient.SetToUseNewestApiVersionAsync();
                this.logger.LogTrace($"API version: {salesforceClient.ApiVersion}");
            }

            return salesforceClient;
        }

        public Task SaveTokenAsync(string name, SalesforceOAuthToken token)
        {
            return this.safeStore.WriteAsync(this.options.SalesforceOAuthTokenStoreCategoryName, name, token);
        }

        public Task<ICollection<string>> EnumerateAsync()
        {
            return this.safeStore.EnumerateCategoryAsync(this.options.SalesforceOAuthTokenStoreCategoryName);
        }
    }
}
