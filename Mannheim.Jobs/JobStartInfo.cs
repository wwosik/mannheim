using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Jobs
{
    public class JobStartInfo
    {
        [JsonProperty(Required = Required.Always)]
        public List<string> Assemblies { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string CommandName { get; set; }

        public bool DisableDebugging { get; set; }
    }
}
