using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static T Build<T>(this IServiceProvider serviceProvider, params object[] args)
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, args);
        }
    }
}
