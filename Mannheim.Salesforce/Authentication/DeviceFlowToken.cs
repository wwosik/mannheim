using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Authentication
{
    public class DeviceFlowToken
    {
        [JsonPropertyName("device_code")]
        public string DeviceCode { get; set; }

        [JsonPropertyName("user_code")]
        public string UserCode { get; set; }

        [JsonPropertyName("verification_uri")]
        public string VerificationUri { get; set; }

        [JsonPropertyName("interval")]
        public int Interval { get; set; }
    }
}
