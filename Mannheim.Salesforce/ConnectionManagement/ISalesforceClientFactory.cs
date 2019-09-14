using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Mannheim.Salesforce.Client.RestApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public interface ISalesforceClientFactory
    {
        Task<SalesforceClient> CreateClientAsync(string name, ApiVersion apiVersion = null);
        Task<SalesforceOAuthToken> GetTokenAsync(string name);
        Task SaveTokenAsync(string name, SalesforceOAuthToken token);
        Task<ICollection<string>> EnumerateAsync();

        Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null);

        Task SaveOAuthConfigurationAsync(SalesforceOAuthConfiguration configuration);
        Task<SalesforceOAuthConfiguration> GetOAuthConfigurationAsync();

        Task<SalesforceAuthenticationClient> CreateAuthenticationClientAsync(Uri uri);
    }
}
