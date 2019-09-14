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

        public CommandFactory(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            this.availableCommands = assembly.GetTypes()
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
