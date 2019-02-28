using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mannheim.DataProtection;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client;
using Mannheim.Salesforce.ConnectionManagement;
using Mannheim.Salesforce.DataProtection;
using Mannheim.XUnit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce
{
    public class TestingServices : TestServicesBase
    {
        private static SalesforceClient cachedClient;

        public TestSalesforceSystemOptions SalesforceOptions { get; }

        public TestingServices(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var builder = new ConfigurationBuilder()
               .AddUserSecrets<TestSalesforceSystemOptions>(false);

            var config = builder.Build();
            this.SalesforceOptions = config.GetSection("Salesforce").Get<TestSalesforceSystemOptions>();
            if (this.SalesforceOptions == null)
            {
                throw new InvalidOperationException("Please configure User Secrets");
            }

        }

        public async Task<SalesforceClient> GetSalesforceClientFromUserSecretsAsync(bool cached = true)
        {
            if (cached && cachedClient != null)
            {
                return cachedClient;
            }

            var authClient = this.Build<SalesforceAuthenticationClient>();
            var token = await authClient.ExchangeUserPasswordForTokenAsync(this.SalesforceOptions.Username, this.SalesforceOptions.Password, this.SalesforceOptions.ApiToken);

            var client = new SalesforceClient(
                this.GetRequiredService<IHttpClientFactory>().CreateClient(), token, this.GetRequiredService<ILogger<SalesforceClient>>());

            if (cached)
            {
                cachedClient = client;
            }

            return client;
        }

        public override void ConfigureTestSpecificServices(IServiceCollection services)
        {
            services.Configure<SalesforceAuthenticationClientOptions>(o =>
          {
              o.SalesforceOAuthConfig = this.SalesforceOptions.SalesforceOAuthConfig;
              o.LoginSystemUri = this.SalesforceOptions.LoginSystemUri;
          });

            services.Configure<SalesforceConfigDataProtectedFileStoreOptions>(o =>
            {
                o.Folder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mannheim.Salesforce.Test"));
                o.ProtectorKey = "Mannheim.Salesforce.Test";
            });

            services.AddDataProtection();
            services.AddSingleton<ISalesforceConfigStore, SalesforceConfigDataProtectedFileStore>();
            services.AddSingleton<SalesforceClientProvider>();
        }
    }
}
