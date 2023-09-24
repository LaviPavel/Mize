var a = new ChainResource<ExchangeRateList, string>();
var val = a.GetValue();
val.Wait();
Console.WriteLine("Hello :)");