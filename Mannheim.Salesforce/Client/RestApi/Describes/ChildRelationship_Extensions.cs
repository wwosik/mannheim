using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mannheim.Salesforce.Client.RestApi.Describes
{
    public partial class ChildRelationship
    {
        [JsonIgnore]
        public DescribeObjectResult TargetObjectHeader { get; set; }

        public static readonly HashSet<string> SystemRelationships = new HashSet<string>{
            "Feeds", "Histories", "Emails", "Shares", "Events", "Personas", "Posts", "Tasks", "TaskRelations"
        };

        [JsonIgnore]
        public bool IsSystem => SystemRelationships.Contains(this.RelationshipName) || DescribeObjectResult.SystemObjectNames.Contains(this.ChildSObject);
    }
}
