using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi
{
    public class QueryResultAnonymousType
    {
        [JsonPropertyName("totalSize")]
        public int TotalSize { get; set; }

        [JsonPropertyName("done")]
        public bool IsDone { get; set; }

        [JsonPropertyName("records")]
        public List<object> Records { get; set; }

        [JsonPropertyName("nextRecordsUrl")]
        public string NextRecordsUrl { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}
