using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Mannheim.Salesforce.Client.RestApi;
using Mannheim.Salesforce.ConnectionManagement;
using Mannheim.Storage;
using Mannheim.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Salesforce.ConnectionManagement
{


    public static class SalesforceClientFactoryExtensions
    {
        public static void AddSalesforceClientFactory(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<SalesforceClientFactory>();
            services.AddSingleton<ISalesforceClientFactory, SalesforceClientFactory>(sp => sp.GetRequiredService<SalesforceClientFactory>());
        }
    }
}
