

using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class ChildRelationship
    {
        [JsonPropertyName("cascadeDelete")]
        public bool CascadeDelete { get; set; }

        [JsonPropertyName("childSObject")]
        public string ChildSObject { get; set; }

        [JsonPropertyName("deprecatedAndHidden")]
        public bool DeprecatedAndHidden { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("junctionIdListNames")]
        public string[] JunctionIdListNames { get; set; }

        [JsonPropertyName("junctionReferenceTo")]
        public string[] JunctionReferenceTo { get; set; }

        [JsonPropertyName("relationshipName")]
        public string RelationshipName { get; set; }

        [JsonPropertyName("restrictedDelete")]
        public bool RestrictedDelete { get; set; }
    }
}
