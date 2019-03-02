using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class DescribeObjectResult
    {
        [JsonProperty("name")]
        public string ApiName { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("labelPlural")]
        public string LabelPlural { get; set; }

        [JsonProperty("queryable")]
        public bool Queryable { get; set; }

        [JsonProperty("feedEnabled")]
        public bool FeedEnabled { get; set; }

        [JsonProperty("keyPrefix")]
        public string KeyPrefix { get; set; }

        [JsonProperty("custom")]
        public bool Custom { get; set; }

        [JsonProperty("fields")]
        public List<FieldDescription> Fields { get; set; }

        [JsonProperty("recordTypeInfos")]
        public List<RecordTypeInfo> RecordTypeInfos { get; set; }

        [JsonProperty("childRelationships")]
        public List<ChildRelationship> ChildRelationships { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public override string ToString() => $"{this.Label ?? "<NO LABEL>"} ({this.ApiName})";
    }
}
