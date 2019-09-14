using Mannheim.Salesforce.Client.RestApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceClientFactory<T> : ISalesforceClientFactory<T>
    {
        private readonly ISalesforceClientFactory salesforceClientFactory;
        private readonly IServiceProvider serviceProvider;

        public SalesforceClientFactory(ISalesforceClientFactory salesforceClientFactory, IServiceProvider serviceProvider)
        {
            this.salesforceClientFactory = salesforceClientFactory;
            this.serviceProvider = serviceProvider;
        }

        public async Task<T> CreateClientAsync(string name, ApiVersion apiVersion = null)
        {
            var salesforceClient = await this.salesforceClientFactory.CreateClientAsync(name, apiVersion);
            return ActivatorUtilities.CreateInstance<T>(this.serviceProvider, salesforceClient);
        }
    }
}
