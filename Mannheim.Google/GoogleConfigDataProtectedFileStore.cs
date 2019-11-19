using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Mannheim.Storage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mannheim.Google
{
    public class GoogleConfigDataProtectedFileStore : IGoogleConfigStore
    {
        private readonly FileSystemJsonSafeStore fileStore;

        public GoogleConfigDataProtectedFileStore(FileSystemJsonSafeStore fileStore)
        {
            this.fileStore = fileStore;
        }

        public Task<GoogleOAuthConfig> GetGoogleOAuthConfigAsync(string name = "__default")
        {
            return this.fileStore.ReadAsync<GoogleOAuthConfig>("oauthConfig", name);
        }

        public Task SaveGoogleOAuthConfigAsync(GoogleOAuthConfig value, string name = "__default")
        {
            return this.fileStore.WriteAsync("oauthConfig", name, value);
        }

        public Task<TokenResponse> GetGoogleTokenAsync(string name = "__default_token")
        {
            return this.fileStore.ReadAsync<TokenResponse>("token", name);
        }

        public Task SaveGoogleTokenAsync(TokenResponse value, string name = "__default_token")
        {
            return this.fileStore.WriteAsync("token", name, value);
        }
    }
}
