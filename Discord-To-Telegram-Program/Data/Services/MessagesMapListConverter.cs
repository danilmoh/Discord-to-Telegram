using DiscordToTelegram.Data.Models;
using System.Collections.Generic;

namespace DiscordToTelegram.Data.Services;

static class MessagesMapListConverter
{
    public static List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>> ConvertToList(Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> messagesMap)
    {
        var list = new List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>>();

        foreach (var pair in messagesMap)
        {
            var key = pair.Key;
            var value = ConvertToList(pair.Value);

            list.Add(new KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>(key, value));
        }

        return list;
    }

    private static List<KeyValuePair<Message, Message>> ConvertToList(Dictionary<Message, Message> messages)
    {
        var list = new List<KeyValuePair<Message, Message>>();
        foreach (var pair in messages)
        {
            list.Add(pair);
        }

        return list;
    }

    public static Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> ConvertToMap
        (List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>> messagesList)
    {
        var dict = new Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>>();

        foreach (var pair in messagesList)
        {
            var key = pair.Key;
            var valueDictionary = new Dictionary<Message, Message>();

            foreach (var messagePair in pair.Value)
            {
                valueDictionary.Add(messagePair.Key, messagePair.Value);
            }

            dict.Add(key, valueDictionary);
        }

        return dict;
    }
}