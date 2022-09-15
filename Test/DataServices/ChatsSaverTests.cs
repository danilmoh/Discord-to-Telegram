
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data;
using System.Text.Json;
using System.IO;
using System;

namespace Test.DataServices;

public class ChatsSaverTests
{
    private readonly ITestOutputHelper output;

    private readonly string currentDirectory;
    private readonly string testChatsDataPath;
    private readonly string defaultTelegramChatsSavePath;
    private readonly string defaultDiscordChatsSavePath;

    public ChatsSaverTests(ITestOutputHelper output)
    {
        this.output = output;
        currentDirectory = System.IO.Directory.GetCurrentDirectory();
        testChatsDataPath = currentDirectory + "/DataServices/ChatsSaverChatsData/";

        defaultTelegramChatsSavePath = currentDirectory + "/CurrentChats/TelegramChats.json";
        defaultDiscordChatsSavePath = currentDirectory + "/CurrentChats/DiscordChats.json";
    }

    [Fact]
    public void Save_ListOfChatsWithSavePath_SavesCorrectly()
    {
        var savePath = testChatsDataPath + "ProvidedChatsPath.json";
        var exampleChats = RandomChatsGenerator.GenerateRandomChats();

        ChatsSaver.Save(exampleChats, savePath);
        var savedChats = LoadChats(savePath);

        Assert.Equal(exampleChats, savedChats);
    }

    [Fact]
    public void Save_ListOfChatsWithTelegramPathType_SavesCorrectly()
    {
        var exampleChats = RandomChatsGenerator.GenerateRandomChats();

        ChatsSaver.Save(exampleChats, BotType.TELEGRAM);
        var savedChats = LoadChats(defaultTelegramChatsSavePath);

        Assert.Equal(exampleChats, savedChats);
    }

    [Fact]
    public void Save_ListOfChatsWithDiscordPathType_SavesCorrectly()
    {
        var exampleChats = RandomChatsGenerator.GenerateRandomChats();

        ChatsSaver.Save(exampleChats, BotType.DISCORD);
        var savedChats = LoadChats(defaultDiscordChatsSavePath);

        Assert.Equal(exampleChats, savedChats);
    }

    [Fact]
    public void Save_ListOfChatsWithIncorrectSavePath_ThrowsFileNotFoundException()
    {
        var savePath = testChatsDataPath + "NonExistingFile.json";
        var exampleChats = RandomChatsGenerator.GenerateRandomChats();

        var action = () => ChatsSaver.Save(exampleChats, savePath);

        Assert.Throws<FileNotFoundException>(action);
    }

    [Fact]
    public void Save_NullListArgWithSavePath_ThrowsArgumentNullException()
    {
        var savePath = testChatsDataPath + "ProvidedChatsPath.json";

        var action = () => ChatsSaver.Save(null, savePath);

        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Save_ChatsListWithNullSavePath_ThrowsArgumentNullException()
    {
        var exampleChats = RandomChatsGenerator.GenerateRandomChats();

        var action = () => ChatsSaver.Save(exampleChats, null);

        Assert.Throws<ArgumentNullException>(action);
    }


    private static List<Chat>? LoadChats(string loadPath)
    {
        var data = System.IO.File.ReadAllText(loadPath);
        return JsonSerializer.Deserialize<List<Chat>>(data);
    }

}

