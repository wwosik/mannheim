using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceIdTokenUserInfo
    {
        [JsonPropertyName("at_hash")]
        public string Hash { get; set; }

        [JsonPropertyName("sub")]
        public string Sub { get; set; }

        [JsonPropertyName("preferred_username")]
        public string Username { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
