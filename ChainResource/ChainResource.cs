public class ChainResource<T1, T2> : IChainResource where T1 : IMizeCache<T2>, new()
{
    private readonly IEnumerable<IStore<T2>> _stores;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

    public ChainResource()
    {
        _stores = new T1().Stores;
    }

    public async Task GetValue()
    {
        var storesToBeUpdated = new List<IStore<T2>>();
        T2 value = default(T2);
        
        foreach (var store in _stores)
        {
            if (store.Type == StorageType.Read)
            {
                var isValue = store.GetValue(out value, _timeout);
                if (isValue)
                    break;
            }
            else if (store.Type == StorageType.Write)
            {
                if (DateTime.UtcNow - store.LastUpdated < store.ExpirationTime)
                {
                    var isValue = store.GetValue(out value, _timeout);
                    if (isValue)
                        break;
                }

                storesToBeUpdated.Add(store);
            }
        }
        
        if (value == null)
        {
            throw new Exception("Could not get value from any store");
        }

        storesToBeUpdated.ForEach(store => UpdateStoreAsync(store, value));
    }

    private async Task UpdateStoreAsync(IStore<T2> store, T2 value)
    {
        store.UpdateValue(value).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                throw new Exception($"Could not update store, error: {t.Exception}");
            }
        });
    }
}