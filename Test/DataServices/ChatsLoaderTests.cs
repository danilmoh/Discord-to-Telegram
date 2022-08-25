using Xunit;
using DiscordToTelegram.Exceptions;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Xunit.Abstractions;

public class ChatsLoaderTests
{
    private readonly ITestOutputHelper output;

    private readonly string ProjectDirectory =
        Directory.GetParent(
            Directory.GetCurrentDirectory()
        ).Parent.Parent.ToString() + "/";

    private const string DiscordChatsPath = "/home/danil/Projects/RiderProjects/Discord-To-Telegram/Discord-To-Telegram-Program/LocalData/DiscordChats.json";
    private const string TelegramChatsPath = "/home/danil/Projects/RiderProjects/Discord-To-Telegram/Discord-To-Telegram-Program/LocalData/TelegramChats.json";

    public ChatsLoaderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Load_NonExistingPath_ThrowsFileNotFoundException()
    {
        var nonExistingFile = "/home/hello.json";

        Action outputAction = () => ChatsLoader.Load(nonExistingFile);

        Assert.Throws<FileNotFoundException>(outputAction);
    }

    [Fact]
    public void Load_EmptyData_ReturnsEmptyList()
    {
        this.output.WriteLine(ProjectDirectory);

        var emptyFilePath =
            $"{ProjectDirectory}DataServices/ChatsLoaderChatsData/EmptyFile.json";

        var output = ChatsLoader.Load(emptyFilePath);

        Assert.True(output.Count == 0);
    }

    [Fact]
    public void Load_IncorrectData_ThrowsWrongInputException()
    {
        var wrongDataFilepath = ProjectDirectory + "/DataServices/ChatsLoaderChatsData/WrongDataFile.json";

        Action outputAction = () => ChatsLoader.Load(wrongDataFilepath);

        Assert.Throws<WrongInputException>(outputAction);
    }

    [Fact]
    public void Load_CorrectDataFromPath_ReturnsCorrectData()
    {
        var dataPath = "/home/danil/Projects/RiderProjects/Discord-To-Telegram/Test/DataServices/ChatsLoaderChatsData/CorrectData.json";
        var expectedChats = new List<Chat>();

        // generating random chats
        for (int i = 0; i < 100; i++)
        {
            var random = new Random();

            var positive = random.Next(0, 2) == 1;
            var value = random.NextInt64();
            var chatId = positive ? value : -1 * value;

            var chatName = $";chat - -+ {i};";

            expectedChats.Add(new Chat(chatId, chatName));
        }
        // saving chats to the file
        SaveChats(expectedChats, dataPath);

        var resultChats = ChatsLoader.Load(dataPath);

        Assert.Equal(expectedChats, resultChats);

    }

    private static void SaveChats(List<Chat> chats, string filePath)
    {
        if (!File.Exists(filePath))
            return;


        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        var serialized = JsonSerializer.Serialize(chats, jsonOptions);

        File.WriteAllText(filePath, serialized);
    }
}
