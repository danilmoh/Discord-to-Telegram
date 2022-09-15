
using System;
using System.Collections.Generic;
using DiscordToTelegram.Data.Models;
static class RandomMessagesMapGenerator
{
    private static int chatNameNumber;
    public static Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>> Generate()
    {
        var chatsMessagesDict = new Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>>();

        for (int i = 0; i < 10; i++)
        {
            var srcChat = GenerateRandomChat();
            var destChat = GenerateRandomChat();

            var chatsPair = new KeyValuePair<Chat, Chat>(srcChat, destChat);
            var messagesDict = new Dictionary<Message, Message>();

            for (int j = 0; j < 100; j++)
            {
                var srcMessage = GenerateRandomMessage();
                var destMessage = GenerateRandomMessage();
                messagesDict.Add(srcMessage, destMessage);
            }

            chatsMessagesDict.Add(chatsPair, messagesDict);
        }

        return chatsMessagesDict;
    }

    private static Chat GenerateRandomChat()
    {
        var random = new Random();
        var chatId = GenerateRandomId();
        var chatName = $"; --test_name--{chatNameNumber++} ;";
        var chat = new Chat(chatId, chatName);

        return chat;
    }

    private static long GenerateRandomId()
    {
        var random = new Random();
        var positive = random.Next(0, 2) == 1;
        var id = random.NextInt64();
        var resultId = positive ? id : -1 * id;

        return resultId;
    }

    private static Message GenerateRandomMessage()
    {
        var chatId = GenerateRandomId();
        var id = GenerateRandomId();
        var message = new Message()
        {
            ChatId = chatId,
            Id = id
        };

        return message;
    }
}
