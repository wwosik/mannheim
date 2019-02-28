using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class User : SObject
    {
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool? IsActive { get; set; }

        public Contact Contact { get; set; }
        public string ContactId{get;set;}

    }
}
