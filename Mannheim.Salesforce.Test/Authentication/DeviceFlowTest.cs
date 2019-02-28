using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mannheim.Salesforce.ConnectionManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce.Authentication
{
    public class DeviceFlowTest
    {
        private readonly TestingServices services;
        private readonly ILogger<DeviceFlowTest> logger;

        public DeviceFlowTest(ITestOutputHelper output)
        {
            this.services = new TestingServices(output);
            this.logger = this.services.GetRequiredService<ILogger<DeviceFlowTest>>();
        }

        [Fact]
        public async Task Start()
        {
            var authClient = this.services.Build<SalesforceAuthenticationClient>();
            var token = await authClient.StartDeviceFlowAsync();

            this.logger.LogInformation(JsonConvert.SerializeObject(token));
        }

        [SkippableFact]
        public async Task Complete()
        {
            var deviceCode = this.services.SalesforceOptions.DeviceCode;
            Skip.If(string.IsNullOrEmpty(deviceCode), "Skipping because DeviceCode is missing");

            var authClient = this.services.Build<SalesforceAuthenticationClient>();
            var provider = this.services.GetRequiredService<SalesforceClientProvider>();

            var token = await authClient.ExchangeDeviceCodeForTokenAsync(deviceCode);
            this.logger.LogInformation(JsonConvert.SerializeObject(token));

            await provider.ConfigStore.SaveSalesforceTokenAsync(token);
            await provider.ConfigStore.SaveSalesforceOAuthConfigurationAsync(this.services.SalesforceOptions.SalesforceOAuthConfig);

        }
    }
}
