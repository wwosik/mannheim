using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi
{
    public class QueryResultAnonymousType
    {
        [JsonProperty("totalSize")]
        public int TotalSize { get; set; }

        [JsonProperty("done")]
        public bool IsDone { get; set; }

        [JsonProperty("records")]
        public List<JObject> Records { get; set; }

        [JsonProperty("nextRecordsUrl")]
        public string NextRecordsUrl { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
