public class ExchangeRateList : IMizeCache<string>
{
    public IEnumerable<IStore<string>> Stores { get; }

    public ExchangeRateList()
    {
        Stores = new List<IStore<string>>
        {
            new MemoryStore<string>(null),
            new FileStore<string>("ExchangeRateStore.json", null),
            new WebService<string>(null)
        };
    }
}