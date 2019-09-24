using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Cli
{
    public interface ICommand
    {
        Task RunAsync();
    }
}
