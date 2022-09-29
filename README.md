# Discord-to-Telegram
Program that forwards messages from Discord server channels to Telegram channels. Uses Discord.Net, Telegram.Bot, Telegram.Extensions.Polling packages

In order for the program to work, you need to:
1. Add your Discord bot to the server with channels you want to forward
2. Add your Telegram bot to the channels where you want to receive Discord messages
3. Specify the Discord and Telegram bot tokens in the `Program/BotTokens/DiscordBotToken.txt` and `Program/BotTokens/TelegramBotToken.txt`
5. Specify the names of the source and destination chats in in the `Program/ForwardOptions.txt` file the following way:

```
(source chat a, source chat b)(destination chat 1, destination chat 2)

(source chat a, etc)(destination chat c, etc)
```

One line represents the source chats (in the first brackets) in which messages will be copied to the destination chats (in the second brackets). You can specify only a part of the chat name, but the inaccuracy can lead to the program choosing the wrong chat if the bot has chats with similar names.
The file may contain any number of such lines, but repeating one destination for the same source can result in repeated messages.

6. Execute the Program.cs file
