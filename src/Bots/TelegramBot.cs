using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

public class TelegramBot
{
    private CancellationTokenSource cts;
    private TelegramBotClient _client;
    private List<long> chats = ChatIdSaverLoader.Load();

    public TelegramBot()
    {
        Init();
    }
    public async Task Init()
    {

        _client = new TelegramBotClient(System.IO.File.ReadAllText("./BotTokens/TelegramBotToken.txt"));
        var me = await _client.GetMeAsync();

        cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = { }
        };

        _client.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );
    }

    public void Stop()
    {
        cts.Cancel();
        ChatIdSaverLoader.Save(chats);
    }

    public async void SendMessageToAllChats(string messageText)
    {
        chats.ForEach(c => _client.SendTextMessageAsync(
            text: messageText,
            chatId: c
        ));
    }

    async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        if (update.Type is UpdateType.Message && update.Message is not null)
        {
            var message = update.Message.Text;
            var chatId = update.Message.Chat.Id;

            if(!chats.Contains(chatId))
                chats.Add(chatId);
        }
    }

    Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiException => $"Telegram API Exception: [{apiException.ErrorCode}]",
            _ => exception.Message
        };

        System.Console.WriteLine($"{errorMessage}\n{exception.ToString()}");
        return Task.CompletedTask;
    }

}