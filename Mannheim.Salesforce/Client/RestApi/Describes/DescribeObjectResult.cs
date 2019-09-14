using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class DescribeObjectResult
    {
        [JsonPropertyName("name")]
        public string ApiName { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("labelPlural")]
        public string LabelPlural { get; set; }

        [JsonPropertyName("queryable")]
        public bool Queryable { get; set; }

        [JsonPropertyName("feedEnabled")]
        public bool FeedEnabled { get; set; }

        [JsonPropertyName("keyPrefix")]
        public string KeyPrefix { get; set; }

        [JsonPropertyName("custom")]
        public bool Custom { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldDescription> Fields { get; set; }

        [JsonPropertyName("recordTypeInfos")]
        public List<RecordTypeInfo> RecordTypeInfos { get; set; }

        [JsonPropertyName("childRelationships")]
        public List<ChildRelationship> ChildRelationships { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        public override string ToString() => $"{this.Label ?? "<NO LABEL>"} ({this.ApiName})";
    }
}
