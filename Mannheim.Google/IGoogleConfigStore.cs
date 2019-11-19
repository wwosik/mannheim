using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;

namespace Mannheim.Google
{
    public interface IGoogleConfigStore
    {
        Task<GoogleOAuthConfig> GetGoogleOAuthConfigAsync(string name = "default");
        Task SaveGoogleOAuthConfigAsync(GoogleOAuthConfig value, string name = "default");
        Task<TokenResponse> GetGoogleTokenAsync(string name = "__default_token");
        Task SaveGoogleTokenAsync(TokenResponse value, string name = "__default_token");
    }
}
