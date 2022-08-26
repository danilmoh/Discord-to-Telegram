using Xunit;
using Xunit.Abstractions;
using System.IO;
using DiscordToTelegram.Data;
using DiscordToTelegram.Bots.Services;
using DiscordToTelegram.Exceptions;

namespace Test.Bots.Services;

public class TokenLoaderTests
{
    private readonly ITestOutputHelper output;
    public TokenLoaderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void GetToken_CorrectPath_LoadsTokenCorrectly()
    {
        var tokenPath = Directory.GetCurrentDirectory() + "/Bots/Services/TestCorrectToken.txt";
        var expected = ReadToken(tokenPath);

        var actual = TokenLoader.GetToken(tokenPath);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetToken_EmptyFile_ThrowsEmptyInputException()
    {
        var tokenPath = Directory.GetCurrentDirectory() + "/Bots/Services/EmptyFile.txt";

        var action = () => TokenLoader.GetToken(tokenPath);

        Assert.Throws<DiscordToTelegram.Exceptions.EmptyInputException>(action);
    }

    [Fact]
    public void GetToken_NonExistingFile_ThrowsFileNotFoundException()
    {
        var tokenPath = Directory.GetCurrentDirectory() + "/Bots/Services/NonExistingFile.txt";

        var action = () => TokenLoader.GetToken(tokenPath);

        Assert.Throws<System.IO.FileNotFoundException>(action);
    }

    [Fact]
    public void GetToken_NullArg_ThrowsArgumentNullException()
    {

        var action = () => TokenLoader.GetToken(null);

        Assert.Throws<System.ArgumentNullException>(action);
    }

    [Fact]
    public void GetToken_DiscordType_ReturnsCorrectToken()
    {
        var exampleToken = "jX5A7%mqY9M9MsQdjoG";
        var discordBotTokenPath = Directory.GetCurrentDirectory() + "/BotTokens/DiscordBotToken.txt";
        WriteToken(discordBotTokenPath, exampleToken);

        var actualToken = TokenLoader.GetToken(BotType.DISCORD);

        Assert.Equal(exampleToken, actualToken);

        ClearFile(discordBotTokenPath);

    }

    [Fact]
    public void GetToken_TelegramType_ReturnsCorrectToken()
    {
        var exampleToken = "jX5A7%mqY9M9MsQdjoG";
        var discordBotTokenPath = Directory.GetCurrentDirectory() + "/BotTokens/TelegramBotToken.txt";
        WriteToken(discordBotTokenPath, exampleToken);

        var actualToken = TokenLoader.GetToken(BotType.TELEGRAM);

        Assert.Equal(exampleToken, actualToken);

        ClearFile(discordBotTokenPath);
    }

    private static void WriteToken(string path, string token)
    {
        File.WriteAllText(path, token);
    }

    private static void ClearFile(string path)
    {
        File.WriteAllText(path, "");
    }

    private static string ReadToken(string path)
    {
        return File.ReadAllLines(path)[0];
    }
}