using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mannheim.Cli
{
    public class CommandFactory : ICommandFactory
    {
        private readonly List<CommandInfo> availableCommands;

        public CommandFactory(Assembly assembly) : this(new[] { assembly })
        {
        }

        public CommandFactory(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            this.availableCommands = assemblies.Where(a => a != null).SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetCustomAttributes(typeof(CommandAttribute), false).Cast<CommandAttribute>().Select(ca => new CommandInfo(t, ca)))
                .ToList();
        }

        public string AvailableCommandsText => string.Join(", ", availableCommands.Select(c => c.Name).OrderBy(n => n));

        public CommandInfo FindCommand(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            name = name.ToUpperInvariant();
            return availableCommands.FirstOrDefault(t => t.IsMatch(name));
        }
    }
}
