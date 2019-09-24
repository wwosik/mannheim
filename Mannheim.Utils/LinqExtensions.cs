using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<IList<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            List<T> batch = null;
            foreach (var item in items)
            {
                if (batch == null)
                {
                    batch = new List<T>();
                }
                else if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<T>();
                }

                batch.Add(item);
            }

            if (batch != null)
            {
                yield return batch;
            }
        }
    }
}
