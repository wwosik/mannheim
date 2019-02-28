using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mannheim.DataProtection;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Mannheim.Salesforce.ConnectionManagement;
using Mannheim.Salesforce.DataProtection;

namespace Mannheim.Salesforce
{
    public static class TestingServices
    {
        private static readonly ServiceProvider servicesProvider;
        private static SalesforceClient cachedClient;

        public static TestSalesforceSystemOptions SalesforceOptions { get; }

        static TestingServices()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<TestSalesforceSystemOptions>(false);

            var config = builder.Build();
            SalesforceOptions = config.GetSection("Salesforce").Get<TestSalesforceSystemOptions>();

            var services = new ServiceCollection();
            services.AddHttpClient();

            services.Configure<SalesforceAuthenticationClientOptions>(o =>
            {
                o.SalesforceOAuthConfig = SalesforceOptions.SalesforceOAuthConfig;
                o.LoginSystemUri = SalesforceOptions.LoginSystemUri;
            });

            services.Configure<SalesforceConfigDataProtectedFileStoreOptions>(o =>
            {
                o.Folder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mannheim.Salesforce.Test"));
                o.ProtectorKey = "Mannheim.Salesforce.Test";
            });

            services.AddDataProtection();
            services.AddSingleton<ISalesforceConfigStore, SalesforceConfigDataProtectedFileStore>();
            services.AddSingleton<SalesforceClientProvider>();

            servicesProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return servicesProvider.GetRequiredService<T>();
        }

        public static T Build<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(servicesProvider);
        }

        public static async Task<SalesforceClient> GetSalesforceClientFromUserSecretsAsync(bool cached = true)
        {
            if (cached && cachedClient != null)
            {
                return cachedClient;
            }

            var authClient = Build<SalesforceAuthenticationClient>();
            var token = await authClient.ExchangeUserPasswordForTokenAsync(SalesforceOptions.Username, SalesforceOptions.Password, SalesforceOptions.ApiToken);

            var client = new SalesforceClient(
                GetService<IHttpClientFactory>().CreateClient(), token, GetService<ILogger<SalesforceClient>>());

            if (cached)
            {
                cachedClient = client;
            }

            return client;
        }
    }
}
