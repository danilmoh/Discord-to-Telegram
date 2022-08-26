using System.Collections.Generic;
using System;
using DiscordToTelegram.Data.Models;

namespace Test.DataServices;

static class RandomChatsGenerator
{
    public static List<Chat> GenerateRandomChats()
    {
        var chats = new List<Chat>();

        for (int i = 0; i < 100; i++)
        {
            var random = new Random();

            var positive = random.Next(0, 2) == 1;
            var value = random.NextInt64();
            var chatId = positive ? value : -1 * value;

            var hasName = random.Next(0, 2) == 1;
            var chatName = $";chat - -+ {i};";

            if (hasName)
                chats.Add(new Chat(chatId));
            else
                chats.Add(new Chat(chatId, chatName));
        }

        return chats;
    }
}