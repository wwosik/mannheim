using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Salesforce.Client.RestApi.StandardDataObjects
{
    public class LoginHistory : SObject
    {
        public string ApiType { get; set; }
        public string ApiVersion { get; set; }
        public string Application { get; set; }
        public DateTime LoginTime { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }
}
