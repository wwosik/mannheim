using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class Organization : SObject
    {
        public string Name { get; set; }
        public string OrganizationType { get; set; }
        public string InstanceName { get; set; }
    }
}
