using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public interface ISalesforceConfigStore
    {
        Task<SalesforceOAuthConfiguration> GetSalesforceOAuthConfigurationAsync(string name = "__default");
        Task SaveSalesforceOAuthConfigurationAsync(SalesforceOAuthConfiguration value, string name = "__default");
        Task<SalesforceOAuthToken> GetSalesforceTokenAsync(string name = "__default");
        Task SaveSalesforceTokenAsync(SalesforceOAuthToken value, string name = "__default");
    }
}
