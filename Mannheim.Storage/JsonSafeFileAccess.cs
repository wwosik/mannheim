using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public class JsonSafeFileAccess
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        private readonly IDataProtector dataProtector;
        private readonly JsonSerializerOptions options;

        public JsonSafeFileAccess(IDataProtector dataProtector, JsonSerializerOptions options = null)
        {
            this.dataProtector = dataProtector;
            this.options = options ?? DefaultOptions;
        }

        public Task WriteAsync(string path, object obj)
        {
            var serialized = JsonSerializer.Serialize(obj, this.options);
            var protectedSerialized = this.dataProtector.Protect(serialized);
            File.WriteAllText(path, protectedSerialized);
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync<T>(string path)
        {
            if (!File.Exists(path)) return Task.FromResult(default(T));

            var protectedSerialized = File.ReadAllText(path);
            var serialized = this.dataProtector.Unprotect(protectedSerialized);
            return Task.FromResult(JsonSerializer.Deserialize<T>(serialized, this.options));
        }
    }
}
