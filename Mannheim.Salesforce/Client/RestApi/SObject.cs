using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Mannheim.Utils;


namespace Mannheim.Salesforce.Client.RestApi
{
    public class SObject
    {
        [JsonPropertyName("attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }


        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Id} {this.AdditionalData.AsString()}";
        }
    }    
}
