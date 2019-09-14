using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Storage
{
    public interface IObjectStore
    {
        Task WriteAsync(string category, string key, object obj);
        Task<T> ReadAsync<T>(string category, string key);
        Task<ICollection<string>> EnumerateCategoryAsync(string category);
    }
}
