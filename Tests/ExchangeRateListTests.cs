using FluentAssertions;

namespace Tests;

public class ExchangeRateListTests
{
    [Fact]
    public void ReadAndReadWriteStores_NotExpired_GetValue()
    {
        var chain = new ChainResource<ExchangeRateList, string>();
        var value = chain.GetValue().Result;
        
        value.Should().NotBeNull();
    }
}