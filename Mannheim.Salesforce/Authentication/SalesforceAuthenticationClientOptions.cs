using System;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceAuthenticationClientOptions
    {
        public SalesforceOAuthConfiguration SalesforceOAuthConfig { get; set; }
        public Uri LoginSystemUri { get; set; }
    }
}
