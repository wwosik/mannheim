using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mannheim.Salesforce.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Salesforce.ConnectionManagement
{
    public class SalesforceConnectionProviderTest
    {
        private readonly ITestOutputHelper output;

        public SalesforceConnectionProviderTest(ITestOutputHelper output)
        {
            this.output = output;
        }
    }
}
