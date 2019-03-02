using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public class DescribeSObjectsResult
    {
        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        [JsonProperty("maxBatchSize")]
        public string MaxBatchSize { get; set; }

        [JsonProperty("sobjects")]
        public List<DescribeObjectResult> SObjects { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public override string ToString() => $"{this.SObjects?.Count ?? 0} objects";
    }
}
