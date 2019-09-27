using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Cli
{
    public class OriginalArguments
    {
        private readonly string[] args;

        public OriginalArguments(string[] args)
        {
            this.args = args;
        }

        public string this[int index] => this.args.Length <= index ? "" : this.args[index];

        public IEnumerable<string> AllFromIndex(int index)
        {
            for (var i = index; i < args.Length; i++)
            {
                yield return args[i];
            }
        }
    }
}
