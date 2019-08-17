using System;
using System.Collections.Generic;

namespace Sdk.NETCore.Domain.Concurrency
{
    public interface IConcurrentEnumerator<T> : IDisposable
    {
        ConcurrentEnumeratorItem<T> GetNext();
    }

    public class ConcurrentEnumerator<T> : IConcurrentEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly object _syncLock;

        public ConcurrentEnumerator(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable?.GetEnumerator() ?? throw new ArgumentNullException(nameof(_enumerator));
            _syncLock = new object();
        }

        public ConcurrentEnumeratorItem<T> GetNext()
        {
            lock (_syncLock)
            {
                bool hasItems = _enumerator.MoveNext();
                var value = hasItems ? _enumerator.Current : default;

                return new ConcurrentEnumeratorItem<T>(hasItems, value);
            }
        }

        public void Dispose() => _enumerator.Dispose();
    }
}