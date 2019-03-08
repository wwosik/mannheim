using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Mannheim.Jobs
{
    public interface ICommandServicesConfigurator
    {
        void Initialize(ServiceCollection services);
    }
}
