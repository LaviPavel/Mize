public interface IMizeCache<T>
{
    IEnumerable<IStore<T>> Stores { get; }
}