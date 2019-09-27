using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class FieldPermissions : SObject
    {
        public string ParentId { get; set; }
        public string SobjectType { get; set; }
        public string Field { get; set; }
        public bool PermissionsRead { get; set; }
        public bool PermissionsEdit { get; set; }
    }
}
