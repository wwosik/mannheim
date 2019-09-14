using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public interface IStringStore
    {
        Task AddAsync(string category, params string[] items);
        Task<IEnumerable<string>> EnumerateAsync(string category);
        Task<bool> RemoveAsync(string category, string item);
    }
}
