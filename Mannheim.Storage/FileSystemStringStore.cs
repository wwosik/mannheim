using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public class FileSystemStringStore : IStringStore
    {
        private readonly ILogger<FileSystemStringStore> logger;
        private readonly DirectoryInfo directoryInfo;

        public FileSystemStringStore(ILogger<FileSystemStringStore> logger, DirectoryInfo directoryInfo)
        {
            this.logger = logger;
            this.directoryInfo = directoryInfo;
            this.logger.LogTrace($"FileSystemStringStore configured at {directoryInfo.FullName}");
        }

        public Task<IEnumerable<string>> EnumerateAsync(string category)
        {
            this.logger.LogTrace($"Enumerating {category}");
            return this.ReadAsync(category);
        }

        public async Task<bool> RemoveAsync(string category, string item)
        {
            this.logger.LogTrace($"Removing item from {category}");
            var items = (await this.ReadAsync(category)).ToList();
            var removed = items.Remove(item);
            if (removed) await SaveAsync(category, items);
            return removed;
        }

        public async Task AddAsync(string category, params string[] items)
        {
            this.logger.LogTrace($"Adding {items.Length} items to {category}");
            var existingItems = new HashSet<string>(await this.ReadAsync(category));
            var added = false;
            foreach (var item in items) { added = added || existingItems.Add(item); }
            if (added)
            {
                await this.SaveAsync(category, existingItems);
            }
        }

        private async Task<IEnumerable<string>> ReadAsync(string category)
        {
            var lines = await File.ReadAllLinesAsync(GetFile(category));
            return lines.Where(l => l.Length > 0).Select(l => l.Replace("\\n", "\n", StringComparison.InvariantCulture).Replace("\\r", "\r", StringComparison.InvariantCulture));
        }

        private string GetFile(string category)
        {
            return Path.Combine(this.directoryInfo.FullName, category + ".txt");
        }

        private Task SaveAsync(string category, IEnumerable<string> items)
        {
            return File.WriteAllLinesAsync(GetFile(category), items.Select(i => i.Replace("\n", "\\n", StringComparison.InvariantCulture).Replace("\r", "\\r", StringComparison.InvariantCulture)));
        }
    }

}