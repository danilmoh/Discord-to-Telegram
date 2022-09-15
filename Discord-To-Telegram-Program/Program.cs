using DiscordToTelegram.Bots;

namespace DiscordToTelegram;
public class Program
{
    public static async Task Main(string[] args)
    {
        var telegramBot = new TelegramBot();
        await telegramBot.StartAsync();
        await new DiscordBot(telegramBot).StartAsync();
    }
}
