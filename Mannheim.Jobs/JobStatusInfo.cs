using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Jobs
{
    public class JobStatusInfo
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime StartedAt { get; set; }

        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime LastUpdate { get; set; }
        
        public string Status { get; set; }

        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? CompletedAt { get; set; }
    }
}
