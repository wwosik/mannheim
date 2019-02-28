using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mannheim.DataProtection;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.ConnectionManagement;

namespace Mannheim.Salesforce.DataProtection
{
    public class SalesforceConfigDataProtectedFileStore : ISalesforceConfigStore
    {
        private readonly DataProtectedFileStore fileStore;

        public SalesforceConfigDataProtectedFileStore(IDataProtectionProvider dataProtectionProvider, IOptions<SalesforceConfigDataProtectedFileStoreOptions> options, ILogger<SalesforceConfigDataProtectedFileStore> logger)
        {
            this.fileStore = new DataProtectedFileStore(dataProtectionProvider, options.Value, logger);
        }

        public Task<SalesforceOAuthConfiguration> GetSalesforceOAuthConfigurationAsync()
        {
            return this.fileStore.LoadJsonAsync<SalesforceOAuthConfiguration>("__SalesforceOAuthConfiguration");
        }

        public Task SaveSalesforceOAuthConfigurationAsync(SalesforceOAuthConfiguration value)
        {
            return this.fileStore.SaveJsonAsync("__SalesforceOAuthConfiguration", value);
        }

        public Task<SalesforceOAuthToken> GetSalesforceTokenAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "__default";
            }

            return this.fileStore.LoadJsonAsync<SalesforceOAuthToken>(name);
        }

        public Task SaveSalesforceTokenAsync(SalesforceOAuthToken value, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "__default";
            }

            return this.fileStore.SaveJsonAsync(name, value);
        }
    }
}
