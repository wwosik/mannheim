using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Mannheim.XUnit
{
    public class XunitTestServices : TestServicesBase
    {
        public XunitTestServices(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        public override void ConfigureTestSpecificServices(IServiceCollection services)
        {

        }
    }
}
