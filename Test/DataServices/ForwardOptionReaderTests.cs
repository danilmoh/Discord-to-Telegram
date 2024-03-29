using Xunit;
using Xunit.Abstractions;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test.DataServices;

public class ForwardOptionReaderTests
{

    private readonly ITestOutputHelper output;
    private readonly string currentDirectory;
    private readonly string inputsPath;


    public ForwardOptionReaderTests(ITestOutputHelper output)
    {
        this.output = output;
        currentDirectory = Directory.GetCurrentDirectory();
        inputsPath = currentDirectory + "/DataServices/ForwardOptionReaderInputs/";

    }

    [Fact]
    public void Save_NoInput_ThrowsEmptyInputException()
    {
        var fileNameNoInput = "Save_NoInput_ThrowsEmptyInputException";
        for (int i = 1; i <= 2; i++)
        {
            var inputFilePath = $"{inputsPath}{fileNameNoInput}{i}.txt";
            Assert.Throws<EmptyInputException>(() => ForwardOptionReader.Read(inputFilePath));
        }

    }


    [Fact]
    public void Save_WrongInput_ThrowsWrongInputException()
    {

        var fileNameWrongInput = "Save_WrongInput_ThrowsWrongInputException";
        for (int i = 1; i <= 4; i++)
        {
            var inputFilePath = $"{inputsPath}{fileNameWrongInput}{i}.txt";
            Assert.Throws<WrongInputException>(() => ForwardOptionReader.Read(inputFilePath));

        }
    }

    [Fact]
    public void Save_NonExistingInputPath_ThrowsFileNotFoundException()
    {
        var path = "/home/helloworld.txt";
        Assert.Throws<FileNotFoundException>(() => ForwardOptionReader.Read(path));
    }

    [Fact]
    public void Save_InputDataFromSpecifiedPath_ReturnsCorrectList()
    {
        var expectedOutput = GetExampleForwardOptions();

        var path = $"{inputsPath}Save_InputDataFromSpecifiedPath_ReturnsCorrectList.txt";

        var resultOutput = ForwardOptionReader.Read(path);

        Assert.True(expectedOutput.Count == resultOutput.Count);

        for (int i = 0; i < expectedOutput.Count; i++)
        {
            Assert.True(expectedOutput[i].Sources.Count == resultOutput[i].Sources.Count);
            Assert.True(expectedOutput[i].Destinations.Count == resultOutput[i].Destinations.Count);

            for (int j = 0; j < expectedOutput[i].Sources.Count; j++)
            {
                Assert.True(expectedOutput[i].Sources[j] == resultOutput[i].Sources[j]);
            }

            for (int j = 0; j < expectedOutput[i].Destinations.Count; j++)
            {
                Assert.True(expectedOutput[i].Destinations[j] == resultOutput[i].Destinations[j]);
            }
        }

    }

    [Fact]
    public void ToString_NonEmptyLists_CorrectOutput()
    {
        var forwardOptions = GetExampleForwardOptions();


        // one string builder is one forwardOption.ToString()
        var expectedOutputs = GetExpectedToStringResults(forwardOptions);

        // comparing expected ToString outputs to the resultOutput
        Assert.Equal(expectedOutputs.Count, forwardOptions.Count);

        for (int i = 0; i < forwardOptions.Count; i++)
        {
            Assert.Equal(forwardOptions[i].ToString(), expectedOutputs[i].ToString());
        }
    }


    private static List<ForwardOption> GetExampleForwardOptions()
    {
        var output = new List<ForwardOption>();
        string src1 = "chat1", src2 = "abra kadabra", src3 = "q";
        var src4 = new List<string>() { "chat1", "chat2", "chat3", "a b c -" };
        var dests1 = new List<string>() { "chat2", "chat3", "chat1" };
        var dests2 = new List<string>() { "abcdef", "ghijk", "lmnop" };
        var dests3 = new List<string>() { "rs", "t u v -" };
        var dests4 = new List<string>() { "a b c -", "general" };

        output.AddRange(new List<ForwardOption>(){
            new ForwardOption(new List<string>() { src1 }, dests1),
            new ForwardOption(new List<string>() { src2 }, dests2),
            new ForwardOption(new List<string>() { src3 }, dests3),
            new ForwardOption(src4 , dests4)
        });

        return output;
    }

    private static List<StringBuilder> GetExpectedToStringResults(List<ForwardOption> forwardOptions)
    {
        var expectedOutputs = new List<StringBuilder>();

        foreach (var option in forwardOptions)
        {
            var expectedOutput = new StringBuilder();
            expectedOutput.Append('(');
            expectedOutputs.Add(expectedOutput);

            for (int i = 0; i < option.Sources.Count; i++)
            {
                expectedOutput.Append(option.Sources[i]);

                if (i == option.Sources.Count - 1)
                {
                    expectedOutput.Append(')');
                    break;
                }

                else
                    expectedOutput.Append(',');
            }

            expectedOutput.Append('(');

            for (int i = 0; i < option.Destinations.Count; i++)
            {

                expectedOutput.Append(option.Destinations[i]);

                if (i == option.Destinations.Count - 1)
                {
                    expectedOutput.Append(')');
                    break;
                }

                else
                    expectedOutput.Append(',');
            }

        }

        return expectedOutputs;
    }

}
