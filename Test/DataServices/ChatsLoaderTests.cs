using Xunit;
using DiscordToTelegram.Exceptions;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Xunit.Abstractions;

namespace Test.DataServices;

public class ChatsLoaderTests
{
    private readonly ITestOutputHelper output;

    private readonly string currentDirectory = Directory.GetCurrentDirectory().ToString() + "/";

    public ChatsLoaderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Load_NonExistingPath_ThrowsFileNotFoundException()
    {
        output.WriteLine(Directory.GetCurrentDirectory());

        var nonExistingFile = "/home/hello.json";

        Action outputAction = () => ChatsLoader.Load(nonExistingFile);

        Assert.Throws<FileNotFoundException>(outputAction);
    }

    [Fact]
    public void Load_EmptyData_ReturnsEmptyList()
    {
        this.output.WriteLine(currentDirectory);

        var emptyFilePath =
            $"{currentDirectory}DataServices/ChatsLoaderChatsData/EmptyFile.json";

        var output = ChatsLoader.Load(emptyFilePath);

        Assert.True(output.Count == 0);
    }

    [Fact]
    public void Load_IncorrectData_ThrowsWrongInputException()
    {
        var wrongDataFilepath = currentDirectory + "/DataServices/ChatsLoaderChatsData/WrongDataFile.json";

        Action outputAction = () => ChatsLoader.Load(wrongDataFilepath);

        Assert.Throws<WrongInputException>(outputAction);
    }

    [Fact]
    public void Load_CorrectDataFromPath_ReturnsCorrectData()
    {
        var dataPath = $"{currentDirectory}DataServices/ChatsLoaderChatsData/CorrectData.json";
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
