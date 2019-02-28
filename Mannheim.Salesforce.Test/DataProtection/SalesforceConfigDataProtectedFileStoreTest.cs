using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mannheim.Salesforce.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce.DataProtection
{
    public class SalesforceConfigDataProtectedFileStoreTest
    {
        private readonly ITestOutputHelper output;

        public SalesforceConfigDataProtectedFileStoreTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task SaveAndRestoreConfig()
        {
            var config = TestingServices.GetService<IOptions<SalesforceAuthenticationClientOptions>>();
            var store = TestingServices.Build<SalesforceConfigDataProtectedFileStore>();
            await store.SaveSalesforceOAuthConfigurationAsync(config.Value.SalesforceOAuthConfig);
            var configFromStore = await store.GetSalesforceOAuthConfigurationAsync();
            Assert.Equal(config.Value.SalesforceOAuthConfig.ConsumerId, configFromStore.ConsumerId);
        }
    }
}
