using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Mannheim.DataProtection
{
    public static class DataProtectedFileStoreExtensions
    {
        public static void AddDataProtectedFileStore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DataProtectedFileStore>();
        }
    }
}
