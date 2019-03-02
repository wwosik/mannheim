using System;
using System.Collections.Generic;
using System.Text;
using Mannheim.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi
{

    public class ApiVersion
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public override string ToString()
        {
            return $"{this.Version} ({this.Label}) {this.Url}" + this.AdditionalData.AsString();
        }
    }
}
