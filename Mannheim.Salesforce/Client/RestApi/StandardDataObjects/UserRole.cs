using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class UserRole : SObject
    {
        public string DeveloperName { get; set; }
        public string Name { get; set; }
        public string ParentRoleId { get; set; }
    }
}
