using Discord;
using Discord.WebSocket;

public class DiscordBot
{
    private DiscordSocketClient _client;
    private TelegramBot _sender;

    public DiscordBot(TelegramBot sender)
    {
        this._sender = sender;

        var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
        _client = new DiscordSocketClient(_config);
        _client.Log += Log;
    }

    public async Task MainAsync()
    {

        var token = System.IO.File.ReadAllText("./BotTokens/DiscordBotToken.txt");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.MessageUpdated += MessageUpdated;
        _client.MessageReceived += MessageReceived;
        Console.ReadLine();
        await _client.StopAsync();
        _sender.Stop();
    }

    private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        // If the message was not in the cache, downloading it will result in getting a copy of `after`.
        var message = await before.GetOrDownloadAsync();
        Console.WriteLine($"{message} -> {after}");
    }

    private async Task MessageReceived(SocketMessage message)
    {
        _sender.SendMessageToAllChats(message.ToString());
    }


    private Task Log(LogMessage log)
    {
        return Task.CompletedTask;
    }

}