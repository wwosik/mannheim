using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public class DescribeSObjectsResult
    {
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }

        [JsonPropertyName("maxBatchSize")]
        public string MaxBatchSize { get; set; }

        [JsonPropertyName("sobjects")]
        public List<DescribeObjectResult> SObjects { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        public override string ToString() => $"{this.SObjects?.Count ?? 0} objects";
    }
}
