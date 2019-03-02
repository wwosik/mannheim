using Newtonsoft.Json;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class ChildRelationship
    {
        [JsonProperty("cascadeDelete")]
        public bool CascadeDelete { get; set; }

        [JsonProperty("childSObject")]
        public string ChildSObject { get; set; }

        [JsonProperty("deprecatedAndHidden")]
        public bool DeprecatedAndHidden { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("junctionIdListNames")]
        public string[] JunctionIdListNames { get; set; }

        [JsonProperty("junctionReferenceTo")]
        public string[] JunctionReferenceTo { get; set; }

        [JsonProperty("relationshipName")]
        public string RelationshipName { get; set; }

        [JsonProperty("restrictedDelete")]
        public bool RestrictedDelete { get; set; }
    }
}
