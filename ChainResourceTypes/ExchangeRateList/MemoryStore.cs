using Mize.ChainResourceTypes;

public class MemoryStore<T> : StoreBase<T>
{
    private T _value;

    public MemoryStore(TimeSpan? expiration) : base(StorageType.Write, expiration ?? TimeSpan.FromHours(1))
    {
        _value = default(T);
    }

    internal override bool Get(out T value, TimeSpan timeout)
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