﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class ObjectPermissions : SObject
    {
        public string ParentId { get; set; }
        public string SobjectType { get; set; }
        public bool PermissionsCreate { get; set; }
        public bool PermissionsRead { get; set; }
        public bool PermissionsEdit { get; set; }
        public bool PermissionsDelete { get; set; }
        public bool PermissionsViewAllRecords { get; set; }
        public bool PermissionsModifyAllRecords { get; set; }
    }
}
