using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mannheim.DataProtection
{
    public class DataProtectedFileStore
    {
        private readonly IDataProtector protector;
        private readonly ILogger logger;
        private readonly DirectoryInfo dataFolder;

        public DataProtectedFileStore(IDataProtectionProvider provider, IOptions<DataProtectedFileStoreOptions> options, ILogger<DataProtectedFileStore> logger)
            : this(provider, options.Value, logger)
        {
        }

        public DataProtectedFileStore(IDataProtectionProvider provider, DataProtectedFileStoreOptions options, ILogger logger)
        {
            if (!options.IsValid)
            {
                throw new ArgumentException("Invalid or empty options!");
            }

            this.dataFolder = options.Folder;

            if (!this.dataFolder.Exists)
            {
                this.dataFolder.Create();
            }

            this.protector = provider.CreateProtector(options.ProtectorKey);
            this.logger = logger;
        }

        public async Task SaveJsonAsync(string key, object obj)
        {
            this.logger.LogTrace($"Saving {key} to protected store");
            var content = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var protectedContent = this.protector.Protect(content);
            await File.WriteAllTextAsync(Path.Combine(this.dataFolder.FullName, key), protectedContent);
        }

        public async Task<T> LoadJsonAsync<T>(string key)
        {
            this.logger.LogTrace($"Loading {key} as {typeof(T).FullName} from protected store");
            string protectedContent;
            try
            {
                protectedContent = await File.ReadAllTextAsync(Path.Combine(this.dataFolder.FullName, key));
            }
            catch (FileNotFoundException ex)
            {
                this.logger.LogError(ex.Message);
                throw new KeyNotFoundException("No data in the store for " + key, ex);
            }

            var content = this.protector.Unprotect(protectedContent);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public override string ToString()
        {
            return $"Protected store at ${this.dataFolder.FullName}";
        }
    }
}
