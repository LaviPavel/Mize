using Mize.ChainResourceTypes;

public class MemoryStore<T> : StoreBase<T>
{
    private T _value;

    public MemoryStore(TimeSpan expiration) : base(StorageType.Write, expiration, false)
    {
        _value = default(T);
    }

    internal override bool Get(out T value)
    {
        value = _value;
        return value != null;
    }

    internal override Task Update(T value)
    {
        _value = value;
        return Task.CompletedTask;
    }
}