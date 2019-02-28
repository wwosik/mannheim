using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mannheim.DataProtection;

namespace Mannheim.Google
{
    public class GoogleConfigDataProtectedFileStore : IGoogleConfigStore
    {
        private readonly DataProtectedFileStore fileStore;

        public GoogleConfigDataProtectedFileStore(IDataProtectionProvider dataProtectionProvider, IOptions<GoogleConfigDataProtectedFileStoreOptions> options, ILogger<GoogleConfigDataProtectedFileStore> logger)
        {
            this.fileStore = new DataProtectedFileStore(dataProtectionProvider, options.Value, logger);
        }

        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync<T>(string key)
        {
            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return this.fileStore.LoadJsonAsync<T>(key);
        }

        public Task StoreAsync<T>(string key, T value)
        {
            return this.fileStore.SaveJsonAsync(key, value);
        }

        public Task<GoogleOAuthConfig> GetGoogleOAuthConfigAsync()
        {
            return this.fileStore.LoadJsonAsync<GoogleOAuthConfig>("__oauth");
        }

        public Task SaveGoogleOAuthConfigAsync(GoogleOAuthConfig value)
        {
            return this.fileStore.SaveJsonAsync("__oauth", value);
        }

        public Task<TokenResponse> GetGoogleTokenAsync(string name = "__default_token")
        {
            return this.fileStore.LoadJsonAsync<TokenResponse>(name);
        }

        public Task SaveGoogleTokenAsync(TokenResponse value, string name = "__default_token")
        {
            return this.fileStore.SaveJsonAsync(name, value);
        }
    }

    public class GoogleConfigDataProtectedFileStoreOptions : DataProtectedFileStoreOptions
    {
    }
}
