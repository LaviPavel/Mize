public interface IStore<T>
{
    StorageType Type { get; }
    TimeSpan Expiration { get; }
    bool GetValue(out T value);
    Task UpdateValue(T value);
}