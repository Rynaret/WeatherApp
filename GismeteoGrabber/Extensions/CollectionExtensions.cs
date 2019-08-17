using Sdk.NETCore.Domain.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GismeteoGrabber.Extensions
{
    public static class CollectionExtensions
    {
        public static async Task<TResult[]> ParallelForEachAsync<T, TResult>(this IEnumerable<T> source,
            Func<T, Task<TResult>> action, int degreeOfParallelism, CancellationToken cancellationToken = default)
        {
            degreeOfParallelism = Math.Min(degreeOfParallelism, source.Count());
            using (var enumerator = new ConcurrentEnumerator<T>(source))
            {
                var tasks = Enumerable.Range(0, degreeOfParallelism)
                    .Select(async x =>
                    {
                        var list = new List<TResult>();
                        for (var item = enumerator.GetNext(); item.HasValue; item = enumerator.GetNext())
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return list;
                            }
                            var value = await action(item.Value);
                            list.Add(value);
                        }
                        return list;
                    });

                var results = await Task.WhenAll(tasks);
                cancellationToken.ThrowIfCancellationRequested();
                return results.SelectMany(x => x).ToArray();
            }
        }
    }
}
