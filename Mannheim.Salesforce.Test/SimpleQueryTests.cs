using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.Client.RestApi.StandardDataObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce
{
    public class SimpleQueryTests
    {
        private readonly TestingServices services;
        private readonly ILogger logger;

        public SimpleQueryTests(ITestOutputHelper output)
        {
            this.services = new TestingServices(output);
            this.logger = this.services.GetRequiredService<ILogger<SimpleQueryTests>>();
        }

        [Fact]
        public async Task FindAnyUser()
        {
            var salesforceClient = await this.services.GetSalesforceClientFromUserSecretsAsync();
            var result = await salesforceClient.QueryAsync<User>("SELECT Name FROM User LIMIT 1");
            this.logger.LogInformation(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task CountUsers()
        {
            var salesforceClient = await this.services.GetSalesforceClientFromUserSecretsAsync();
            var result = await salesforceClient.QueryAnonymousTypeAndContinueAsync("SELECT count(id) UserCount FROM User", new { UserCount = 0 });
            Assert.NotEmpty(result);
            Assert.NotEqual(0, result[0].UserCount);
            this.logger.LogInformation(result[0].UserCount.ToString());
        }
    }
}
