using System;

namespace Mannheim.Cli
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name, Type options = null)
        {
            this.Name = name;
            this.Options = options;
        }

        public string Name { get; set; }
        public Type Options { get; set; }
    }
}
