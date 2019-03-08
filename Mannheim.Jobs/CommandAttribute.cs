using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Jobs
{
    public class CommandAttribute : Attribute
    {
        public string CommandServicesConfigurator { get; set; }
        public string JobStartInfoType { get; set; }
    }
}
