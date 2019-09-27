using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class Group : SObject
    {
        public string DeveloperName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string RelatedId { get; set; }
    }
}
