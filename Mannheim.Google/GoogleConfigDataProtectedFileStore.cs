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

        public Task<T> ReadAsync<T>(string name)
        {
            return this.fileStore.ReadAsync<T>("misc", name);
        }

        public Task WriteAsync<T>(string name, T content)
        {
            return this.fileStore.WriteAsync("misc", name, content);
        }

        public Task StoreAsync<T>(string key, T value)
        {
            return this.fileStore.WriteAsync("google", key, value);
        }

        public Task DeleteAsync<T>(string key)
        {
            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return this.fileStore.ReadAsync<T>("google", key);
        }

        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }
    }
}
