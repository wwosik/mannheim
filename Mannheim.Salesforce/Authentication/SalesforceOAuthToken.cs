using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceOAuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("instance_url")]
        public string InstanceUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        [JsonIgnore]
        public SalesforceIdTokenUserInfo IdTokenUserInfo
        {
            get
            {
                if (this.IdToken == null)
                {
                    return null;
                }

                try
                {
                    var split = this.IdToken.Split('.');
                    if (split.Length < 2) return null;
                    var encodedText = split[1];
                    var necessaryPad = (3 - encodedText.Length % 3) % 3; // 0 => 0, 1 => 2, 2=>1 necessary because of Microsoft silly parser
                    encodedText = encodedText.PadRight(encodedText.Length + necessaryPad, '=');
                    var converted = Convert.FromBase64String(encodedText);
                    var serialized = Encoding.ASCII.GetString(converted);
                    return JsonConvert.DeserializeObject<SalesforceIdTokenUserInfo>(serialized);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
