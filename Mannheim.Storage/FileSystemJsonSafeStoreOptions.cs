using System.IO;

namespace Mannheim.Storage
{
    public class FileSystemJsonSafeStoreOptions
    {
        public DirectoryInfo Directory { get; set; }
        public string ProtectionKey { get; set; }
    }
}
