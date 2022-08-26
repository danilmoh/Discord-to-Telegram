
using Discord;
using Discord.WebSocket;

namespace DiscordToTelegram.Bots;
public class DiscordBot
{

    private DiscordSocketClient _client;
    public static Task Main(string[] args) => new DiscordBot().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;


    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}