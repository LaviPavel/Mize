using FluentAssertions;
using Moq;

namespace Tests;

public class StoreTest
{
    private string testString = "test";

    [Fact]
    public async Task Store_UpdateValue()
    {
        //Arrange
        var store = new Mock<IStore<string>>();

        //Act
        await store.Object.UpdateValue(testString);

        //Assert
        store.Verify(s => s.UpdateValue(testString), Times.Once);
    }

    [Fact]
    public async Task Store_GetValue()
    {
        //Arrange
        var store = new Mock<IStore<string>>();
        store.Setup(s => s.GetValue(out testString)).Returns(true);

        //Act
        var isValueReady = store.Object.GetValue(out var value);

        //Assert
        isValueReady.Should().BeTrue();
    }
}