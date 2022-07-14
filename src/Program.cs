
public class Program
{
    public static async Task Main(string[] args)
    {
        TelegramBot telegramBot = new TelegramBot();
        await new DiscordBot(telegramBot).MainAsync();
    }

}

