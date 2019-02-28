using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mannheim.Salesforce.ConnectionManagement;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce.Authentication
{
    public class DeviceFlowTest
    {
        private readonly ITestOutputHelper output;

        public DeviceFlowTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Start()
        {
            var authClient = TestingServices.Build<SalesforceAuthenticationClient>();
            var token = await authClient.StartDeviceFlowAsync();
            this.output.WriteLine(JsonConvert.SerializeObject(token));
        }

        [SkippableFact]
        public async Task Complete()
        {
            var deviceCode = TestingServices.SalesforceOptions.DeviceCode;
            Skip.If(string.IsNullOrEmpty(deviceCode), "Skipping because DeviceCode is missing");

            var authClient = TestingServices.Build<SalesforceAuthenticationClient>();
            var provider = TestingServices.GetService<SalesforceClientProvider>();

            var token = await authClient.ExchangeDeviceCodeForTokenAsync(deviceCode);
            this.output.WriteLine(JsonConvert.SerializeObject(token));

            await provider.ConfigStore.SaveSalesforceTokenAsync(token);
            await provider.ConfigStore.SaveSalesforceOAuthConfigurationAsync(TestingServices.SalesforceOptions.SalesforceOAuthConfig);

        }
    }
}
