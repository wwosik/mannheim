using Mannheim.Salesforce.Authentication;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceClientFactoryOptions
    {
        public string SalesforceOAuthTokenStoreCategoryName { get; set; } = nameof(SalesforceOAuthToken);
        public string SalesforceOAuthConfigurationCategoryName { get; set; } = nameof(SalesforceOAuthConfiguration);
        public string SalesforceOAuthConfigurationKey { get; set; } = "__default";

    }
}
