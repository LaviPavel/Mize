using Mize.ChainResourceTypes;

public class WebService<T> : StoreBase<T>
{
    private string _url = "https://openexchangerates.org/api/latest.json?app_id=3e2d71e83a7f47f5aad21258bd32e35d&prettyprint=false&show_alternative=false";
    private HttpClient _client;
    
    public WebService(HttpClient? client) : base(StorageType.Read)
    {
        _client = client ?? new HttpClient();
    }

    internal override bool Get(out T value, TimeSpan timeout)
    {
        var response = _client.GetAsync(_url).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        if (response.IsSuccessStatusCode)
        {
            value = (T)Convert.ChangeType(content, typeof(T));
            return true;
        }
        
        Console.WriteLine($"Error: {response.StatusCode}, Message: {content}");
        value = default(T);
        return false;
    }

    internal override Task Update(T value)
    {
        throw new NotImplementedException();
    }
}