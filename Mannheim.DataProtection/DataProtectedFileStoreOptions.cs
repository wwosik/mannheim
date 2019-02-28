using System.IO;

namespace Mannheim.DataProtection
{
    public class DataProtectedFileStoreOptions
    {
        public DirectoryInfo Folder { get; set; }
        public string ProtectorKey { get; set; }

        public bool IsValid => this.Folder != null && !string.IsNullOrEmpty(this.ProtectorKey);

        public override string ToString()
        {
            return $"DataProtectedFileStoreOptions {this.Folder?.FullName} :: {this.ProtectorKey}";
        }
    }
}
