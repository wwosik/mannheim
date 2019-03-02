using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mannheim.Utils
{
    public class JsonFileStore : IKeyValueStore
    {
        private readonly ILogger logger;
        private readonly DirectoryInfo baseDir;

        public JsonFileStore(ILogger logger, DirectoryInfo baseDir)
        {
            this.logger = logger;
            this.baseDir = baseDir;
        }

        public void EnsureDirectory()
        {
            if (!this.baseDir.Exists)
            {
                this.logger.LogDebug("Creating folder " + this.baseDir.FullName);
                this.baseDir.Create();
            }
        }

        public async Task<T> GetAsync<T>(string name)
        {
            var path = $"{this.baseDir.FullName}/{name}.json";
            this.logger.LogTrace($"Retrieving {typeof(T).Name} from {path}...");
            var contentToDeserialize = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(contentToDeserialize);
        }

        public Task SaveAsync<T>(string name, T obj)
        {
            return this.SaveAsync(name, obj, Formatting.Indented);
        }

        public async Task SaveAsync<T>(string name, T obj, Formatting formatting)
        {
            var path = $"{this.baseDir.FullName}/{name}.json";
            this.logger.LogTrace($"Saving {typeof(T).Name} into {path}...");
            var serializedContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
            await File.WriteAllTextAsync(path, serializedContent);
        }

        public JsonFileStore GetChildStore(string name, ILogger childLogger = null)
        {
            return new JsonFileStore(childLogger ?? this.logger, new DirectoryInfo(Path.Combine(this.baseDir.FullName, name)));
        }
    }
}
