using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi
{
    public class QueryResult<T>
    {
        [JsonProperty("done")]
        public bool Done { get; set; }

        [JsonProperty("totalSize")]
        public int TotalSize { get; set; }

        [JsonProperty("records")]
        public List<T> Records { get; set; }

        [JsonProperty("nextRecordsUrl")]
        public string NextRecordsUrl { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public override string ToString()
        {
            var rowCount = this.Records?.Count ?? 0;
            return $"{rowCount} records of total {this.TotalSize} for {typeof(T).Name}. Done: {this.Done}";
        }
    }
}
