public interface IStore<T>
{
    StorageType Type { get; }
    TimeSpan ExpirationTime { get; }
    DateTime LastUpdated { get; }
    bool GetValue(out T value, TimeSpan timeout);
    Task UpdateValue(T value);
}