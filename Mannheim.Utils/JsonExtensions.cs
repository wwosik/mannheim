using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Mannheim.Utils
{
    public static class JsonExtensions
    {
        public static string AsString(this IDictionary<string, object> additionalData)
        {
            if (additionalData == null || additionalData.Count == 0)
            {
                return "";
            }

            return " " + string.Join("; ", additionalData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        public static string ToJsonString(this object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
    }
}
