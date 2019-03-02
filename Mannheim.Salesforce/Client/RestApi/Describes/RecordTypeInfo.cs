using Newtonsoft.Json;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public class RecordTypeInfo
    {
        [JsonProperty("master")]
        public bool Master { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("developerName")]
        public string DeveloperName { get; set; }

        [JsonProperty("name")]
        public string Label { get; set; }

        [JsonProperty("recordTypeId")]
        public string Id { get; set; }
    }
}
