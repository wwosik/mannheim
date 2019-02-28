using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceIdTokenUserInfo
    {
        [JsonProperty("at_hash")]
        public string Hash { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("preferred_username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
