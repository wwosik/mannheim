using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public class FileSystemJsonStore : IObjectStore
    {
        private readonly DirectoryInfo directory;
        private readonly ILogger logger;
        public JsonSerializerOptions DefaultOptions { get; } = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [ActivatorUtilitiesConstructor]
        public FileSystemJsonStore(DirectoryInfo directory, ILogger<FileSystemJsonStore> logger) : this(directory, (ILogger)logger)
        {
        }

        public FileSystemJsonStore(DirectoryInfo directory, ILogger logger)
        {
            this.directory = directory;
            this.logger = logger;
        }

        private string GetFilePath(string category, string key)
            => Path.Combine(directory.FullName, category, key + ".json");

        public Task WriteAsync(string category, string key, object obj)
        {
            try
            {
                this.logger.LogTrace($"Saving {category} {key}");
                this.directory.CreateSubdirectory(category);
                return this.WriteAsync(GetFilePath(category, key), obj);
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
                return this.ReadAsync<T>(path);
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

            var files = directory.EnumerateFiles("*.json", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false
            }).Select(f => f.Name.Replace(".json", ""))
            .ToArray();

            return Task.FromResult<ICollection<string>>(files);
        }

        private Task WriteAsync(string path, object obj)
        {
            var serialized = JsonSerializer.Serialize(obj, DefaultOptions);
            return File.WriteAllTextAsync(path, serialized);
        }

        private async Task<T> ReadAsync<T>(string path)
        {
            if (!File.Exists(path)) return default;

            var serialized = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<T>(serialized, DefaultOptions);
        }
    }
}
