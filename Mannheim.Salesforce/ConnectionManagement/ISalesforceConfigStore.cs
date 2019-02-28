using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public interface ISalesforceConfigStore
    {
        Task<SalesforceOAuthConfiguration> GetSalesforceOAuthConfigurationAsync();
        Task SaveSalesforceOAuthConfigurationAsync(SalesforceOAuthConfiguration value);
        Task<SalesforceOAuthToken> GetSalesforceTokenAsync(string name = null);
        Task SaveSalesforceTokenAsync(SalesforceOAuthToken value, string name = null);
    }
}
