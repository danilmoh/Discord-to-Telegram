using Xunit;
using Xunit.Abstractions;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Data;
using System.IO;
using System.Collections.Generic;

namespace Test.DataServices;

public class MessagesLoaderTests
{
    private readonly string currentDirectory;
    private readonly string inputsPath;
    private ITestOutputHelper output;

    public MessagesLoaderTests(ITestOutputHelper output)
    {
        this.output = output;

        currentDirectory = Directory.GetCurrentDirectory();
        inputsPath = currentDirectory + "/DataServices/MessagesLoaderInputs/";
    }

    [Fact]
    public void Load_IncorrectPath_ShouldThrowFileNotFoundException()
    {
        var nonExistingPath = "/home/NonExistingFile.json";

        var action = () => MessagesLoader.Load(nonExistingPath);

        Assert.Throws<FileNotFoundException>(action);
    }

    [Fact]
    public void Load_WrongDataPath_ShouldThrowWrongInputException()
    {
        var wrongFilePath = inputsPath + "WrongData.json";

        var action = () => MessagesLoader.Load(wrongFilePath);

        Assert.Throws<DiscordToTelegram.Exceptions.WrongInputException>(action);
    }

    [Fact]
    public void Load_EmptyFilePath_ShouldReturnEmptyDictionary()
    {
        var emptyFilePath = inputsPath + "EmptyFile.json";

        var output = MessagesLoader.Load(emptyFilePath);

        Assert.Equal(output, new Dictionary<KeyValuePair<Chat, Chat>, Dictionary<Message, Message>>());
    }


    [Fact]
    public void Load_CorrectFilePathProvided_ShouldReturnCorrectData()
    {
        var path = inputsPath + "CorrectData.json";
        var expected = RandomMessagesMapGenerator.Generate();
        MessagesSaver.Save(expected, path);

        var actual = MessagesLoader.Load(path);

        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected.Keys.Count, actual.Keys.Count);
            Assert.Equal(expected.Values.Count, actual.Values.Count);
        }

    }

}