using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class GroupMember : SObject
    {
        public string GroupId { get; set; }
        public string UserOrGroupId { get; set; }
    }
}
