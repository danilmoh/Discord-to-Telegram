using DiscordToTelegram.Data.Models;
using System.Text.Json;

namespace DiscordToTelegram.Data.Services;

public static class MessagesSaver
{
    private static readonly string s_defaultSavePath = DefaultPaths.Messages;
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

        Save(messagesMap, s_defaultSavePath);
    }


}