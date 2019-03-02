using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mannheim.Utils
{
    public static class IOExtensions
    {
        public static DirectoryInfo BuildSubdirectory(this DirectoryInfo directoryInfo, string name)
        {
            return new DirectoryInfo(Path.Combine(directoryInfo.FullName, name));
        }
    }
}
