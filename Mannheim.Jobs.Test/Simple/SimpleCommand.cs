using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Jobs.Simple
{
    public class SimpleCommand : ICommand
    {
        public SimpleCommand()
        {
        }

        public Task Run()
        {
            Console.WriteLine("Console output");
            Console.Error.WriteLine("Error console output");
            return Task.CompletedTask;
        }
    }
}
