public class ChainResource<T1, T2> : IChainResource<T2> where T1 : IMizeCache<T2>, new()
{
    private readonly IEnumerable<IStore<T2>> _stores;
    private bool _isUpdateInProgress;
    private T2 _tmpValue;

    public ChainResource()
    {
        _stores = new T1().Stores;
    }

    public async Task<T2> GetValue()
    {
        if (_isUpdateInProgress) //todo: edge case, update takes longer than store expiration time
        {
            return _tmpValue;
        }

        var storesToBeUpdated = new List<IStore<T2>>();
        T2 value = default(T2);
        
        foreach (var store in _stores)
        {
            if (store.Type == StorageType.Read)
            {
                var isValue = store.GetValue(out value);
                if (isValue)
                    break;
            }
            else if (store.Type == StorageType.Write)
            {
                var isValue = store.GetValue(out value);
                if (isValue)
                    break;

                storesToBeUpdated.Add(store);
            }
        }
        
        if (value == null)
        {
            throw new Exception("Could not get value from any store");
        }

        storesToBeUpdated.ForEach(store => UpdateStoreAsync(store, value));

        return value;
    }

    private async Task UpdateStoreAsync(IStore<T2> store, T2 value)
    {
        _tmpValue = value;
        _isUpdateInProgress = true;
        store.UpdateValue(value).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                throw new Exception($"Could not update store, error: {t.Exception}");
            }
        });

        _isUpdateInProgress = false;
    }
}