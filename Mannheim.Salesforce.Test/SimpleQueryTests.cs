using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mannheim.Salesforce.Client.RestApi.StandardDataObjects;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce
{
    public class SimpleQueryTests
    {
        private readonly ITestOutputHelper output;

        public SimpleQueryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task FindAnyUser()
        {
            var salesforceClient = await TestingServices.GetSalesforceClientFromUserSecretsAsync();
            var result = await salesforceClient.QueryAsync<User>("SELECT Name FROM User LIMIT 1");
            this.output.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task CountUsers()
        {
            var salesforceClient = await TestingServices.GetSalesforceClientFromUserSecretsAsync();
            var result = await salesforceClient.QueryAnonymousTypeAndContinueAsync("SELECT count(id) UserCount FROM User", new { UserCount= 0 });
            Assert.NotEmpty(result);
            Assert.NotEqual(0, result[0].UserCount);
            this.output.WriteLine(result[0].UserCount.ToString());
        }
    }
}
