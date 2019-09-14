using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi
{
    public class QueryResult<T>
    {
        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("totalSize")]
        public int TotalSize { get; set; }

        [JsonPropertyName("records")]
        public List<T> Records { get; set; }

        [JsonPropertyName("nextRecordsUrl")]
        public string NextRecordsUrl { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        public override string ToString()
        {
            var rowCount = this.Records?.Count ?? 0;
            return $"{rowCount} records of total {this.TotalSize} for {typeof(T).Name}. Done: {this.Done}";
        }
    }
}
