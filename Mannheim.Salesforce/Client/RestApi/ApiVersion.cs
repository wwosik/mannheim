using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Mannheim.Utils;


namespace Mannheim.Salesforce.Client.RestApi
{

    public class ApiVersion
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        public override string ToString()
        {
            return $"{this.Version} ({this.Label}) {this.Url}" + this.AdditionalData.AsString();
        }
    }
}
