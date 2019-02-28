using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client.RestApi.StandardDataObjects;
using Mannheim.Salesforce.ConnectionManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce
{
    public class TestConnection
    {
        private readonly TestingServices services;
        private readonly ILogger<TestConnection> logger;

        public TestConnection(ITestOutputHelper output)
        {
            this.services = new TestingServices(output);
            this.logger = this.services.GetRequiredService<ILogger<TestConnection>>();
        }

        [Fact]
        public async Task CanConnectWithUserPassword()
        {
            var salesforceAuthClient = this.services.Build<SalesforceAuthenticationClient>();
            var options = this.services.SalesforceOptions;
            var token = await salesforceAuthClient.ExchangeUserPasswordForTokenAsync(options.Username, options.Password, options.ApiToken);
            this.logger.LogInformation(JsonConvert.SerializeObject(token));
        }

        [Fact]
        public async Task CanConnectFromStore()
        {
            var clientProvider = this.services.GetRequiredService<SalesforceClientProvider>();
            var client = await clientProvider.GetClientAsync();
            var result = await client.QueryAnonymousTypeAndContinueAsync("SELECT count(id) UserCount FROM User", new { UserCount = 0 });
            Assert.NotEmpty(result);
            Assert.NotEqual(0, result[0].UserCount);
            this.logger.LogInformation(result[0].UserCount.ToString());
        }
    }
}
