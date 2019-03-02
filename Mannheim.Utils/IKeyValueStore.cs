using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Utils
{
    public interface IKeyValueStore
    {
        Task<T> GetAsync<T>(string name);
        Task SaveAsync<T>(string name, T obj);
    }
}
