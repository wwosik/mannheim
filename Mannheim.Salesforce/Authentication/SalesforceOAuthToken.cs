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

                var serialized = Encoding.UTF8.GetString(Convert.FromBase64String(this.IdToken.Split('.')[1]));
                return JsonConvert.DeserializeObject<SalesforceIdTokenUserInfo>(serialized);
            }
        }
    }
}
