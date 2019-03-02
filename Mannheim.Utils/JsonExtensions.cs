using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Utils
{
    public static class JsonExtensions
    {
        public static string AsString(this IDictionary<string, JToken> additionalData)
        {
            if (additionalData == null || additionalData.Count == 0)
            {
                return "";
            }

            return " " + string.Join("; ", additionalData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        public static string ToJsonString(this object obj, Formatting formatting = Formatting.None)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, formatting);
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
    }
}
