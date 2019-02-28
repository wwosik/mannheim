using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Mannheim.Salesforce.Authentication
{
    public class SalesforceOAuthConfiguration
    {
        public string RedirectUrl { get; set; }

        public string ConsumerId { get; set; }

        public string ConsumerSecret { get; set; }

        public string Scope { get; set; } = "api refresh_token";

        public string Prompt { get; set; } = "login";

        public string Display { get; set; } = "page";

        public override string ToString()
        {
            return $"Salesforce OAuth configuration with client id {this.ConsumerId}";
        }

        public Uri GetAuthenticationUrl(Uri serverUrl, string state = null)
        {
            var payload = new Dictionary<string, string> {
                { "response_type", "code" },
                { "client_id", this.ConsumerId },
                { "scope", this.Scope },
                { "display" , this.Display },
                { "prompt", this.Prompt },
                { "redirect_uri", this.RedirectUrl },
            };

            if (state != null)
            {
                payload.Add("state", state);
            }

            var form = new FormUrlEncodedContent(payload);

            return new Uri($"{serverUrl}/services/oauth2/authorize?" + form.ReadAsStringAsync().Result);
        }
    }
}
