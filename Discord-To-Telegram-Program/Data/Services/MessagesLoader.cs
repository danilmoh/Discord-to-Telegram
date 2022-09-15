using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Exceptions;
using System.Text.Json;

namespace DiscordToTelegram.Data.Services;

public static class MessagesLoader
{
    public static Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Message file not found!");

        var content = File.ReadAllText(path);

        if (new FileInfo(path).Length == 0 || string.IsNullOrWhiteSpace(content))
        {
            return new Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>>();
        }

        try
        {
            var text = File.ReadAllText(path);
            var deserialized = JsonSerializer.Deserialize<List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>>>(text);
            var dict = MessagesMapListConverter.ConvertToMap(deserialized);

            return dict;
        }
        catch
        {
            throw new WrongInputException("Wrong messages data provided!");
        }

        return new Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>>();

    }

    public static Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> Load()
    {
        throw new NotImplementedException();
    }
}