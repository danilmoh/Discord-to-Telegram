
using Xunit;
using DiscordToTelegram.Data.Models;

namespace Test.DataModels;

public class ChatTests
{
    [Fact]
    public void Equals_WithNullArg_ReturnsFalse()
    {
        var chat = new Chat(123);

        Assert.False(chat.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentTypeArg_ReturnsFalse()
    {
        var chat = new Chat(123);

        Assert.False(chat.Equals(123));
    }

    [Fact]
    public void Equals_ChatWithSameNameButDifferentId_ReturnsFalse()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(321, "abc");

        Assert.False(chat.Equals(chat1));
    }

    [Fact]
    public void Equals_ChatWithSameIdButDifferentName_ReturnsTrue()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(123, "efg");

        Assert.True(chat.Equals(chat1));
    }

    [Fact]
    public void Equals_ChatWithSameIdButNullName_ReturnsTrue()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(123);

        Assert.True(chat.Equals(chat1));
    }

    [Fact]
    public void Equals_ChatWithSameIdButBothNullNames_ReturnsTrue()
    {
        var chat = new Chat(1234);
        var chat1 = new Chat(1234);

        Assert.True(chat.Equals(chat1));
    }

    [Fact]
    public void Equals_ChatWithDifferentIdBothNullNames_ReturnsFalse()
    {
        var chat = new Chat(100);
        var chat1 = new Chat(123);

        Assert.False(chat.Equals(chat1));
    }

    [Fact]
    public void GetHashCode_ChatWithoutName_CorrectHashCode()
    {
        var chat = new Chat(123);

        Assert.Equal(chat.GetHashCode(), 123.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ChatWithoutName_DifferentHashCodeThanTest()
    {
        var chat = new Chat(123);

        Assert.NotEqual(chat.GetHashCode(), 321.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ChatWithName_CorrectHashCode()
    {
        var chat = new Chat(123, "abc");

        Assert.Equal(chat.GetHashCode(), 123.GetHashCode());
    }

    [Fact]
    public void GetHashCode_TwoChatsWithDifferentNames_SameHashCode()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(123, "def");

        Assert.Equal(chat.GetHashCode(), chat1.GetHashCode());
    }

    [Fact]
    public void GetHashCode_TwoChatsSameNames_SameHashCode()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(123, "abc");

        Assert.Equal(chat.GetHashCode(), chat1.GetHashCode());
    }

    [Fact]
    public void GetHashCode_OneChatWithNullName_SameHashCode()
    {
        var chat = new Chat(123, "abc");
        var chat1 = new Chat(123);

        Assert.Equal(chat.GetHashCode(), chat1.GetHashCode());
    }

    [Fact]
    public void GetHashCode_TwoChatsNullNames_SameHashCode()
    {
        var chat = new Chat(123);
        var chat1 = new Chat(123);

        Assert.Equal(chat.GetHashCode(), chat1.GetHashCode());
    }

    [Fact]
    public void GetHashCode_OneChatWithNullName_DifferentHashCode()
    {
        var chat = new Chat(1234, "abc");
        var chat1 = new Chat(123);

        Assert.NotEqual(chat.GetHashCode(), chat1.GetHashCode());
    }

    [Fact]
    public void GetHashCode_TwoChatsWithNullNames_DifferentHashCode()
    {
        var chat = new Chat(1234);
        var chat1 = new Chat(123);

        Assert.NotEqual(chat.GetHashCode(), chat1.GetHashCode());
    }
}
