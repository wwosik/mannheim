using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mannheim.Cli
{
    public class CommandInfo
    {
        
        private readonly Type commandType;
        private readonly CommandAttribute commandAttribute;

        public CommandInfo(Type commandType, CommandAttribute commandAttribute)
        {
            this.commandType = commandType;
            this.commandAttribute = commandAttribute;
        }

        public string Name => this.commandAttribute.Name;

        public bool IsMatch(string name)
        {
            return this.commandAttribute.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (this.commandAttribute.Options != null)
            {
                services.AddOptions();
                var configureOptionsType = typeof(ConfigureNamedOptions<>).MakeGenericType(this.commandAttribute.Options);
                var configureOptionsInterface = typeof(IConfigureOptions<>).MakeGenericType(this.commandAttribute.Options);

                var optionsInstance = Activator.CreateInstance(configureOptionsType, null, (Action<object>)(o => configuration.Bind(o)));

                services.AddSingleton(configureOptionsInterface, optionsInstance);
            }

            var configureMethod = this.commandType.GetMethod("ConfigureServices", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            if (configureMethod != null)
            {
                configureMethod.Invoke(null, new object[] { configuration, services });
            }
        }

        public ICommand CreateInstance(ServiceProvider serviceProvider)
        {
            return (ICommand)ActivatorUtilities.CreateInstance(serviceProvider, this.commandType);
        }
    }
}
