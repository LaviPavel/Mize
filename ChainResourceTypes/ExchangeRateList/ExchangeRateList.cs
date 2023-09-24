public class ExchangeRateList : IMizeCache<string>
{
    public IEnumerable<IStore<string>> Stores { get; }

    public ExchangeRateList()
    {
        Stores = new List<IStore<string>>
        {
            new MemoryStore<string>(TimeSpan.FromHours(1)),
            new FileStore<string>("ExchangeRateStore.json", TimeSpan.FromHours(4)),
            new WebService<string>(null)
        };
    }
}