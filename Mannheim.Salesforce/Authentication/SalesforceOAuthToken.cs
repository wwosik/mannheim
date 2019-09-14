using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceOAuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("instance_url")]
        public string InstanceUrl { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("issued_at")]
        public string IssuedAt { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

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
                    return JsonSerializer.Deserialize<SalesforceIdTokenUserInfo>(serialized);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
