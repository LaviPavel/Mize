public interface IChainResource<T>
{
    Task<T> GetValue();
}