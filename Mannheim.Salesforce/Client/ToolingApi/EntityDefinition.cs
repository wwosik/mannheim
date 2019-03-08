using System;
using System.Collections.Generic;
using System.Text;
using Mannheim.Salesforce.Client.RestApi;

namespace Mannheim.Salesforce.Client.ToolingApi
{
    public class EntityDefinition : SObject
    {
        public string QualifiedApiName { get; set; }
        public string Description { get; set; }
        public string InternalSharingModel { get; set; }
    }
}
