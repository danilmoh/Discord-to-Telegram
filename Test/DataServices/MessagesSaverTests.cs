
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using DiscordToTelegram.Data;

namespace Test.DataServices;

public class MessagesMapSaverTests
{
    private readonly ITestOutputHelper output;

    private static readonly string s_currentDirectory = Directory.GetCurrentDirectory().ToString() + "/";
    private static readonly string s_currentTestDataDirectory = $"{s_currentDirectory}DataServices/MessagesSaverMessagesData/";

    public MessagesMapSaverTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Save_IncorrectPath_ShouldThrowFileNotFoundException()
    {
        var path = $"{s_currentTestDataDirectory}NonExistingFile.txt";
        var chats = RandomMessagesMapGenerator.Generate();

        var action = () => MessagesSaver.Save(chats, path);

        Assert.Throws<FileNotFoundException>(action);
    }

    [Fact]
    public void Save_NullDict_ShouldThrowFileNotFoundException()
    {
        var path = $"{s_currentTestDataDirectory}NonExistingFile.txt";

        var action = () => MessagesSaver.Save(null, path);

        Assert.Throws<FileNotFoundException>(action);
    }

    [Fact]
    public void Save_CorrectDataProvidedPath_SavesCorrectly()
    {
        var expected = RandomMessagesMapGenerator.Generate();
        var expectedList = new List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>>();
        foreach (var pair in expected)
        {
            var key = pair.Key;
            var value = new List<KeyValuePair<Message, Message>>();

            foreach (var subPair in value)
            {
                var newPair = new KeyValuePair<Message, Message>(subPair.Key, subPair.Value);
                value.Add(newPair);
            }

            expectedList.Add(new KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>(key, value));
        }

        var path = $"{s_currentTestDataDirectory}Save_CorrectDataProvidedPath_SavesCorrectly.json";

        MessagesSaver.Save(expected, path);
        var actualList = LoadMessages(path);

        for (int i = 0; i < expectedList.Count; i++)
        {
            var expectedKey = expectedList[i].Key;
            var actualKey = actualList[i].Key;
            Assert.Equal(expectedKey, actualKey);

            for (int j = 0; j < expectedList[i].Value.Count; j++)
            {
                var expectedValue = expectedList[i].Value[j];
                var actualValue = actualList[i].Value[j];
                Assert.Equal(expectedValue, actualValue);
            }
        }
    }

    [Fact]
    public void Save_CorrectDataProvided_SavesCorrectly()
    {
        var expected = RandomMessagesMapGenerator.Generate();
        var expectedList = new List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>>();
        foreach (var pair in expected)
        {
            var key = pair.Key;
            var value = new List<KeyValuePair<Message, Message>>();

            foreach (var subPair in value)
            {
                var newPair = new KeyValuePair<Message, Message>(subPair.Key, subPair.Value);
                value.Add(newPair);
            }

            expectedList.Add(new KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>(key, value));
        }

        var defaultPath = DefaultPaths.Messages;

        MessagesSaver.Save(expected);
        var actualList = LoadMessages(defaultPath);


        for (int i = 0; i < expectedList.Count; i++)
        {
            var expectedKey = expectedList[i].Key;
            var actualKey = actualList[i].Key;
            Assert.Equal(expectedKey, actualKey);

            for (int j = 0; j < expectedList[i].Value.Count; j++)
            {
                var expectedValue = expectedList[i].Value[j];
                var actualValue = actualList[i].Value[j];
                Assert.Equal(expectedValue, actualValue);
            }
        }
    }

    private static List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>> LoadMessages(string path)
    {
        var text = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<KeyValuePair<KeyValuePair<Chat, Chat>, List<KeyValuePair<Message, Message>>>>>(text);
    }


}