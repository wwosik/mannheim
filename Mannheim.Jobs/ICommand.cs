using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Jobs
{
    public interface ICommand
    {
        Task Run();
    }
}
