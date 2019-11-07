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
        Task<SalesforceClient> CreateClientAsync(string name, ApiVersion apiVersion = null, string oauthConfigName = "__default");
        Task<SalesforceOAuthToken> GetTokenAsync(string name);
        Task SaveTokenAsync(string name, SalesforceOAuthToken token);
        Task<ICollection<(string, SalesforceOAuthToken)>> EnumerateTokensAsync();

        Task<Uri> GetWebFlowUriAsync(Uri loginSystemUri, string state = null);

        Task SaveOAuthConfigurationAsync(SalesforceOAuthConfiguration configuration, string oauthConfigName = "__default");
        Task<SalesforceOAuthConfiguration> GetOAuthConfigurationAsync(string oauthConfigName = "__default");

        Task<SalesforceAuthenticationClient> CreateAuthenticationClientAsync(Uri uri, string oauthConfigName = "__default");
    }
}
