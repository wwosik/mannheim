﻿using System;
using System.Collections.Generic;
using System.Text;
using Mannheim.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Client.RestApi
{
    public class SObject
    {
        public string Id { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Id} {this.AdditionalData.AsString()}";
        }
    }
}
