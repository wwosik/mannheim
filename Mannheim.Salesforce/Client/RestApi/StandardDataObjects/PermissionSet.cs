using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class PermissionSet : SObject
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Profile Profile { get; set; }
    }
}
