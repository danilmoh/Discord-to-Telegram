namespace DiscordToTelegram.Data;

public static class DefaultPaths
{
    public static readonly string TelegramChats = Directory.GetCurrentDirectory() + "/CurrentChats/TelegramChats.json";
    public static readonly string DiscordChats = Directory.GetCurrentDirectory() + "/CurrentChats/DiscordChats.json";
    public static readonly string Messages = Directory.GetCurrentDirectory() + "/CurrentChats/CurrentMessages.json";
    public static readonly string ForwardOptions = Directory.GetCurrentDirectory() + "/ForwardOptions.txt";
    public static readonly string TelegramBotToken = Directory.GetCurrentDirectory() + "/BotTokens/TelegramBotToken.txt";
    public static readonly string DiscordBotToken = Directory.GetCurrentDirectory() + "/BotTokens/DiscordBotToken.txt";

}