using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mannheim.Salesforce.Authentication;
using Mannheim.Salesforce.Client.RestApi.StandardDataObjects;
using Mannheim.Salesforce.ConnectionManagement;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce
{
    public class TestConnection
    {
        private readonly ITestOutputHelper output;

        public TestConnection(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task CanConnectWithUserPassword()
        {
            var salesforceAuthClient = TestingServices.Build<SalesforceAuthenticationClient>();
            var options = TestingServices.SalesforceOptions;
            var token = await salesforceAuthClient.ExchangeUserPasswordForTokenAsync(options.Username, options.Password, options.ApiToken);
            this.output.WriteLine(JsonConvert.SerializeObject(token));
        }

        [Fact]
        public async Task CanConnectFromStore()
        {
            var clientProvider = TestingServices.GetService<SalesforceClientProvider>();
            var client = await clientProvider.GetClientAsync();
            var result = await client.QueryAnonymousTypeAndContinueAsync("SELECT count(id) UserCount FROM User", new { UserCount = 0 });
            Assert.NotEmpty(result);
            Assert.NotEqual(0, result[0].UserCount);
            this.output.WriteLine(result[0].UserCount.ToString());
        }
    }
}
