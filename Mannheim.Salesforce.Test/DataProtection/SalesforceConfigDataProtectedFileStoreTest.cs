using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce.DataProtection
{
    public class SalesforceConfigDataProtectedFileStoreTest
    {
        private readonly TestingServices services;
        private readonly ILogger logger;

        public SalesforceConfigDataProtectedFileStoreTest(ITestOutputHelper output)
        {
            this.services = new TestingServices(output);
            this.logger = this.services.GetRequiredService<ILogger<SalesforceConfigDataProtectedFileStoreTest>>();
        }

        [Fact]
        public async Task SaveAndRestoreConfig()
        {
            var config = this.services.GetRequiredService<IOptions<SalesforceAuthenticationClientOptions>>();
            var store = this.services.Build<SalesforceConfigDataProtectedFileStore>();
            await store.SaveSalesforceOAuthConfigurationAsync(config.Value.SalesforceOAuthConfig);
            var configFromStore = await store.GetSalesforceOAuthConfigurationAsync();
            Assert.Equal(config.Value.SalesforceOAuthConfig.ConsumerId, configFromStore.ConsumerId);
        }
    }
}
