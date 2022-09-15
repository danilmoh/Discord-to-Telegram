using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Data;
using System.Text.Json;

namespace DiscordToTelegram.Data.Services;

public static class MessagesSaver
{
    public static void Save(Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> messagesMap, string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException();

        var listedMap = MessagesMapListConverter.ConvertToList(messagesMap);
        var result = JsonSerializer.Serialize(listedMap);
        File.WriteAllText(path, result);
    }

    public static void Save(Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> messagesMap)
    {
        var path = DefaultPaths.Messages;
        Save(messagesMap, path);
    }


}