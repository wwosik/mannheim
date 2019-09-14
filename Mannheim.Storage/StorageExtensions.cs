using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mannheim.Storage
{
    public static class StorageExtensions
    {
        public static void AddFileSystemJsonSafeStore(this IServiceCollection services, DirectoryInfo secureStoreFolder, string key)
        {
            services.AddDataProtection(o => { o.ApplicationDiscriminator = key; })
                        .PersistKeysToFileSystem(secureStoreFolder);

            services.Configure<FileSystemJsonSafeStoreOptions>(o =>
            {
                o.Directory = secureStoreFolder;
                o.ProtectionKey = key;
            });

            services.AddSingleton<IObjectStore, FileSystemJsonSafeStore>();
        }

        public static void AddFileSystemStringStore(this IServiceCollection services, DirectoryInfo directory)
        {
            services.Configure<FileSystemStringStoreOptions>(o =>
            {
                o.Directory = directory;
            });
            services.AddSingleton<IStringStore, FileSystemStringStore>();

        }
    }
}
