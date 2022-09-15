using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DiscordToTelegram.Bots.Services;
using DiscordToTelegram.Data.Models;
using DiscordToTelegram.Data.Services;
using Discord;
using System.Text.RegularExpressions;

namespace DiscordToTelegram.Bots;

public class TelegramBot
{
    private List<Data.Models.Chat> currentChats;
    private Dictionary<KeyValuePair<Data.Models.Chat, Data.Models.Chat>, Dictionary<Data.Models.Message, Data.Models.Message>> messages;
    private TelegramBotClient client;
    private CancellationTokenSource cts;
    public TelegramBot()
    {
        currentChats = ChatsLoader.Load(Data.BotType.TELEGRAM);

        messages = new Dictionary<KeyValuePair<Data.Models.Chat, Data.Models.Chat>,
            Dictionary<Data.Models.Message, Data.Models.Message>>();
    }

    public async Task StartAsync()
    {
        var token = TokenLoader.GetToken(Data.BotType.TELEGRAM);
        client = new TelegramBotClient(token);
        cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };


        client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await client.GetMeAsync();

        Console.WriteLine($"Start listening for {me.Username}");
    }

    public void Stop()
    {
        cts.Cancel();
        ChatsSaver.Save(currentChats, Data.BotType.TELEGRAM);
    }

    public async Task ForwardMessageAsync(string destinationChatName, Discord.WebSocket.SocketMessage message)
    {
        // selects appropriate destination chat from current chats based on provided name
        // firstly searches for the identical name, if there's no such, then searches for similar one
        var destinationChat = currentChats
                // searching for the same name  
                .FirstOrDefault(chat => chat.Name.Equals(destinationChatName),
                    // searching for the similar name
                    currentChats.FirstOrDefault(chat => chat.Name.Contains(destinationChatName)));

        if (destinationChat is null)
            return;

        Console.WriteLine($"this message has {message.Attachments.Count} attachments in it");


        var sourceChat = new Data.Models.Chat(message.Channel.Id, message.Channel.Name);
        var srcMessage = new Data.Models.Message()
        {
            ChatId = message.Channel.Id,
            Id = message.Id
        };

        if (message.Type == Discord.MessageType.Reply)
        {
            try
            {
                var referenceMessage = new Data.Models.Message()
                {
                    ChatId = message.Reference.ChannelId,
                    Id = (ulong)message.Reference.MessageId
                };
                var kvp = new KeyValuePair<Data.Models.Chat, Data.Models.Chat>(sourceChat, destinationChat);
                var dict = messages[kvp];
                var repliedMessage = dict.GetValueOrDefault(referenceMessage);
                int replyToMessageId = (int)repliedMessage.Id;
                await SendMessageAndAddToDictionary(sourceChat, destinationChat, message, replyToMessageId);

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot send reply to message");
                Console.WriteLine(e.StackTrace);
            }
        }

        await SendMessageAndAddToDictionary(sourceChat, destinationChat, message);
    }

    private async Task SendMessageAndAddToDictionary(Data.Models.Chat srcChat, Data.Models.Chat destChat, Discord.WebSocket.SocketMessage discordMessage)
    {

        if (!string.IsNullOrWhiteSpace(discordMessage.Content))
        {
            var sentMessage = await client.SendTextMessageAsync(
                chatId: new ChatId((long)destChat.Id),
                text: discordMessage.Content,
                cancellationToken: cts.Token
            );

            AddMessageToDictionary(srcChat, destChat, discordMessage, sentMessage);
        }

        discordMessage.Attachments.ToList().ForEach(async a => await SendAttachmentAsync(destChat, a));
    }

    private async Task SendMessageAndAddToDictionary(Data.Models.Chat srcChat, Data.Models.Chat destChat, Discord.WebSocket.SocketMessage discordMessage, int replyToMessageId)
    {
        var sentMessage = await client.SendTextMessageAsync(
            chatId: new ChatId((long)destChat.Id),
            text: discordMessage.Content,
            cancellationToken: cts.Token,
            replyToMessageId: replyToMessageId
        );

        AddMessageToDictionary(srcChat, destChat, discordMessage, sentMessage);

        discordMessage.Attachments.ToList().ForEach(async a => await SendAttachmentAsync(destChat, a));

    }

    private void AddMessageToDictionary(Data.Models.Chat srcChat, Data.Models.Chat destChat, Discord.WebSocket.SocketMessage discordMessage, Telegram.Bot.Types.Message telegramMessage)
    {
        var srcMessage = new Data.Models.Message() { Id = discordMessage.Id, ChatId = discordMessage.Channel.Id };
        var destMessage = new Data.Models.Message() { Id = telegramMessage.MessageId, ChatId = telegramMessage.Chat.Id };
        var srcDestPair = new KeyValuePair<Data.Models.Chat, Data.Models.Chat>(srcChat, destChat);

        if (!messages.ContainsKey(srcDestPair))
        {
            var dictOfMessages = new Dictionary<Data.Models.Message, Data.Models.Message>();
            messages[srcDestPair] = dictOfMessages;
        }

        messages[srcDestPair].Add(srcMessage, destMessage);
    }

    private async Task SendAttachmentAsync(Data.Models.Chat destChat, Attachment attachment)
    {
        var contentType = attachment.ContentType;
        var attachmentType = contentType.Split('/')[0];

        switch (attachmentType)
        {
            case ("image"):
                await client.SendPhotoAsync(
                    chatId: (long)destChat.Id,
                    cancellationToken: cts.Token,
                    photo: attachment.Url
                );
                break;
            case ("audio"):
                await client.SendAudioAsync(
                    chatId: (long)destChat.Id,
                    cancellationToken: cts.Token,
                    audio: attachment.Url
                );
                break;
            case ("video"):
                await client.SendVideoAsync(
                    chatId: (long)destChat.Id,
                    cancellationToken: cts.Token,
                    video: attachment.Url
                );
                break;
            case ("x"):
            case ("text"):
                await client.SendDocumentAsync(
                    chatId: (long)destChat.Id,
                    cancellationToken: cts.Token,
                    document: attachment.Url
                );
                break;
            default:
                Console.WriteLine("ERROR:\nCannot send the attachment of this content type");
                break;
        }
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var post = update.ChannelPost;
        if (post is null)
            return;
        var chatName = post.Chat.Title;

        var chat = new Data.Models.Chat(post.Chat.Id, chatName);
        if (!currentChats.Contains(chat))
            currentChats.Add(chat);

    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}