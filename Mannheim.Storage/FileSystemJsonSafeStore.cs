using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public class FileSystemJsonSafeStore : IObjectStore
    {
        private readonly DirectoryInfo directory;
        private readonly JsonSafeFileAccess fileSystemAccess;
        private readonly ILogger<Storage.FileSystemJsonSafeStore> logger;

        public FileSystemJsonSafeStore(IOptions<FileSystemJsonSafeStoreOptions> options, ILogger<Storage.FileSystemJsonSafeStore> logger, IDataProtectionProvider dataProtectionProvider)
        {
            this.directory = options.Value.Directory;
            this.fileSystemAccess = new JsonSafeFileAccess(dataProtectionProvider.CreateProtector(options.Value.ProtectionKey));
            this.logger = logger;
        }

        private string GetFilePath(string category, string key)
            => Path.Combine(directory.FullName, category, key + ".json.secured");

        public Task WriteAsync(string category, string key, object obj)
        {
            try
            {
                this.logger.LogTrace($"Saving {category} {key}");
                this.directory.CreateSubdirectory(category);
                return this.fileSystemAccess.WriteAsync(GetFilePath(category, key), obj);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to save {category} {key}: {ex.Message}");
                throw;
            }
        }

        public Task<T> ReadAsync<T>(string category, string key)
        {
            try
            {
                this.logger.LogTrace($"Reading {category} {key}");
                var path = GetFilePath(category, key);
                return this.fileSystemAccess.ReadAsync<T>(path);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to read {category} {key}: {ex.Message}");
                throw;
            }
        }

        public Task<ICollection<string>> EnumerateCategoryAsync(string category)
        {
            var directory = new DirectoryInfo(Path.Combine(this.directory.FullName, category));
            if (!directory.Exists) return Task.FromResult<ICollection<string>>(Array.Empty<string>());

            var files = directory.EnumerateFiles("*.json.secured", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false
            }).Select(f => f.Name.Replace(".json.secured", ""))
            .ToArray();

            return Task.FromResult<ICollection<string>>(files);
        }
    }
}
