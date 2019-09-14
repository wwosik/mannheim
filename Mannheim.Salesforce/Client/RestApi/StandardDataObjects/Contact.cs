using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class Contact : SObject
    {
        public Account Account { get; set; }
        public string AccountId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
