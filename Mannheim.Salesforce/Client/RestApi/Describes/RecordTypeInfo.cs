

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public class RecordTypeInfo
    {
        [JsonPropertyName("master")]
        public bool Master { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("developerName")]
        public string DeveloperName { get; set; }

        [JsonPropertyName("name")]
        public string Label { get; set; }

        [JsonPropertyName("recordTypeId")]
        public string Id { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
