using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Mannheim.Salesforce.Authentication
{
    public class DeviceFlowToken
    {
        [JsonProperty("device_code")]
        public string DeviceCode { get; set; }

        [JsonProperty("user_code")]
        public string UserCode { get; set; }

        [JsonProperty("verification_uri")]
        public string VerificationUri { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }
    }
}
