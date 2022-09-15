
using Discord;
using Discord.WebSocket;
using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Data.Services;
using DiscordToTelegram.Bots.Services;

namespace DiscordToTelegram.Bots;
public class DiscordBot
{

    private List<Chat> currentChats;
    private List<ForwardOption> forwardOptions;
    private DiscordSocketClient client;


    private TelegramBot sender;

    public DiscordBot(TelegramBot sender)
    {
        this.sender = sender;
        currentChats = ChatsLoader.Load(Data.BotType.DISCORD);
        forwardOptions = ForwardOptionReader.Read();
    }

    public async Task StartAsync()
    {
        var config = new DiscordSocketConfig { MessageCacheSize = 100 };
        client = new DiscordSocketClient(config);

        client.Log += Log;
        client.MessageReceived += MessageReceived;

        var token = TokenLoader.GetToken(Data.BotType.DISCORD);

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        client.Ready += () =>
        {
            Console.WriteLine("Bot is connetced!");
            return Task.CompletedTask;
        };

        Console.ReadLine();

        sender.Stop();
    }

    private async Task MessageReceived(SocketMessage receivedMessage)
    {

        forwardOptions
            .Where(option => option.Sources.Any
                (source => receivedMessage.Channel.Name.Contains(source)))
            .SelectMany(option => option.Destinations)
            .ToList()
            .ForEach(destination => sender.ForwardMessageAsync(destination, receivedMessage));

        var chat = new Chat(receivedMessage.Channel.Id, receivedMessage.Channel.Name);
        if (!currentChats.Contains(chat))
            currentChats.Add(chat);
    }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}