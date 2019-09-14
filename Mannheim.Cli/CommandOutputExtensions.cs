using System;
using System.Globalization;

namespace Mannheim.Cli
{
    public static class CommandOutputExtensions
    {
        public static void WriteLine(this ICommandOutput output, string pattern, params object[] args)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            output.WriteLine(string.Format(CultureInfo.CurrentCulture, pattern, args));
        }
    }
}
