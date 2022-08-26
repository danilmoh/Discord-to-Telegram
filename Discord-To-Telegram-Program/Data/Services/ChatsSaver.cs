using DiscordToTelegram.Data.Models;
using System.Text.Json;
using System.IO;

namespace DiscordToTelegram.Data.Services;

public static class ChatsSaver
{
    private static readonly string s_telegramChatsPath =
        Directory.GetCurrentDirectory() + "/CurrentChats/TelegramChats.json";

    private static readonly string s_discordChatsPath =
        Directory.GetCurrentDirectory() + "/CurrentChats/DiscordChats.json";

    public static void Save(List<Chat> chats, BotType type)
    {
        var savePath = type == BotType.DISCORD ? s_discordChatsPath : s_telegramChatsPath;
        Save(chats, savePath);
    }

    public static void Save(List<Chat> chats, string path)
    {
        if (chats is null)
        {
            throw new ArgumentNullException(nameof(chats));
        }
        else if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!File.Exists(path))
            throw new FileNotFoundException();

        var options = new JsonSerializerOptions { WriteIndented = true };
        var saved = JsonSerializer.Serialize(chats, options);
        File.WriteAllText(path, saved);
    }
}
