using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.DataProtection;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.ConnectionManagement;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mannheim.Salesforce.DataProtection
{
    public class SalesforceConfigDataProtectedFileStore : ISalesforceConfigStore
    {
        private readonly DataProtectedFileStore fileStore;

        public SalesforceConfigDataProtectedFileStore(IDataProtectionProvider dataProtectionProvider, IOptions<SalesforceConfigDataProtectedFileStoreOptions> options, ILogger<SalesforceConfigDataProtectedFileStore> logger)
        {
            this.fileStore = new DataProtectedFileStore(dataProtectionProvider, options.Value, logger);
        }

        public Task<SalesforceOAuthConfiguration> GetSalesforceOAuthConfigurationAsync(string name = "__default")
        {
            return this.fileStore.LoadJsonAsync<SalesforceOAuthConfiguration>("__SalesforceOAuthConfiguration_" + name);
        }

        public Task SaveSalesforceOAuthConfigurationAsync(SalesforceOAuthConfiguration value, string name = "__default")
        {
            return this.fileStore.SaveJsonAsync("__SalesforceOAuthConfiguration_" + name, value);
        }

        public Task<SalesforceOAuthToken> GetSalesforceTokenAsync(string name = "__default")
            => this.fileStore.LoadJsonAsync<SalesforceOAuthToken>(name);

        public Task SaveSalesforceTokenAsync(SalesforceOAuthToken value, string name = "__default")
          => this.fileStore.SaveJsonAsync(name, value);
    }
}
