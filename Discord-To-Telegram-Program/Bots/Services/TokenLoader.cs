using DiscordToTelegram.Data;
using DiscordToTelegram.Exceptions;

namespace DiscordToTelegram.Bots.Services;


public static class TokenLoader
{
    private static readonly string s_defaultDiscordTokenPath = DefaultPaths.DiscordBotToken;

    private static readonly string s_defaultTelegramTokenPath = DefaultPaths.TelegramBotToken;

    public static string GetToken(string path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        var content = File.ReadAllText(path);
        if (new FileInfo(path).Length == 0 || string.IsNullOrWhiteSpace(content))
        {
            throw new EmptyInputException("No input");
        }
        return File.ReadAllLines(path)[0];
    }

    public static string GetToken(BotType type)
    {
        var tokenPath = type == BotType.DISCORD ? s_defaultDiscordTokenPath : s_defaultTelegramTokenPath;
        return GetToken(tokenPath);
    }
}