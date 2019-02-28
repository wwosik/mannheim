using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Google
{
    public class GoogleOAuthConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }

        public global::Google.Apis.Auth.OAuth2.ClientSecrets ToGoogleApi()
        {
            return new global::Google.Apis.Auth.OAuth2.ClientSecrets
            {
                ClientId = this.ClientId,
                ClientSecret = this.ClientSecret
            };
        }
    }
}
