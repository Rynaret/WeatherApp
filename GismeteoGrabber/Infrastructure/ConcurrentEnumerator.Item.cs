namespace Sdk.NETCore.Domain.Concurrency
{
    public struct ConcurrentEnumeratorItem<T>
    {
        public T Value { get; }

        public bool HasValue { get; }

        public ConcurrentEnumeratorItem(bool hasValue, T value)
        {
            Value = value;
            HasValue = hasValue;
        }
    }
}