using DiscordToTelegram.Data.Models;
using System.Text.Json;
using DiscordToTelegram.Exceptions;
using Xunit.Abstractions;

namespace DiscordToTelegram.Data.Services;

public static class ChatsLoader
{
    private static readonly string s_discordChatsPath = DefaultPaths.DiscordChats;
    private static readonly string s_telegramChatsPath = DefaultPaths.TelegramChats;

    public static List<Chat> Load(string loadPath)
    {
        if (!File.Exists(loadPath))
        {
            throw new FileNotFoundException("Non existing file provided to ChatsLoader");
        }

        var content = File.ReadAllText(loadPath);

        if (string.IsNullOrWhiteSpace(content))
            return new List<Chat>();

        try
        {
            var output = JsonSerializer.Deserialize<List<Chat>>(content);

            if (output is null)
                throw new WrongInputException();

            return output;
        }
        catch (Exception)
        {
            throw new WrongInputException("Wrong input provided to ChatsLoader");
        }
    }

    public static List<Chat> Load(BotType type)
    {
        var chosenPath = (type == BotType.DISCORD) ? s_discordChatsPath : s_telegramChatsPath;

        return Load(chosenPath);
    }
}
