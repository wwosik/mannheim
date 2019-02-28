using Mannheim.Salesforce.Authentication;

namespace Mannheim.Salesforce
{
    public class TestSalesforceSystemOptions : SalesforceAuthenticationClientOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiToken { get; set; }

        public string DeviceCode { get; set; }
    }
}
