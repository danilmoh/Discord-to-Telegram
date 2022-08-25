using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Exceptions;
using System.Linq;

namespace DiscordToTelegram.Data.Services;
public static class ForwardOptionReader
{

    private const string DefaultPath = "Discord-To-Telegram-Program/ForwardOptions.txt";
    public static List<ForwardOption> Read()
    {
        return Read(DefaultPath);
    }

    public static List<ForwardOption> Read(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        var content = File.ReadAllText(path);

        if (new FileInfo(path).Length == 0 || string.IsNullOrWhiteSpace(content))
        {
            throw new EmptyInputException("No input");
        }

        var options = File.ReadAllLines(path);

        var result = options
        .Select(str => str.Remove(str.LastIndexOf(')')))
        .Select(str => str.Remove(0, 1))
        .Select(str => str.Split(")("))
        .Select(arr =>
        {
            try
            {
                var sources = arr[0].Split(',').ToList();
                var destinations = arr[1].Split(',').ToList();
                return new ForwardOption(sources, destinations);
            }
            catch
            {
                throw new WrongInputException();
            }

        })
        .ToList();

        return result;
    }
}
