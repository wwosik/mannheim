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
        public SObjectFieldsSet GetFieldSet() => new SObjectFieldsSet(this.Fields?.Select(f => f.Name));

        public static HashSet<string> SystemObjectNames = new HashSet<string>{
            "AcceptedEventRelation", "ActivityHistory", "Asset", "AttachedContentNotes", "Attachments", "CombinedAttachments", "AttachedContentDocument"
            , "ContentDistribution", "ContentDocumentLink", "ContentVersion", "DeclinedEventRelation", "DuplicateRecordItem"
            , "EntitySubscription", "EventRelation", "NoteAndAttachment", "ProcessInstance", "ProcessInstanceHistory", "RecordAction", "SocialPersona"
            };
    }
}
