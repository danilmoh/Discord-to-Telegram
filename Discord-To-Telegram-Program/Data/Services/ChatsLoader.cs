using DiscordToTelegram.Data.Models;
using System.Text.Json;
using DiscordToTelegram.Exceptions;
using Xunit.Abstractions;

namespace DiscordToTelegram.Data.Services;

public class ChatsLoader
{
    private readonly ITestOutputHelper output;

    private const string DiscordChatsPath = "Discord-To-Telegram-Program/LocalData/DiscordChats.json";
    private const string TelegramChatsPath = "Discord-To-Telegram-Program/LocalData/TelegramChats.json";

    public ChatsLoader(ITestOutputHelper output)
    {
        this.output = output;
    }

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
        var chosenPath = (type == BotType.DISCORD) ? DiscordChatsPath : TelegramChatsPath;

        return Load(chosenPath);
    }
}
