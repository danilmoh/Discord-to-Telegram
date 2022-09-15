namespace DiscordToTelegram.Data.Models;
public record Message
{
    public decimal Id { get; set; }
    public decimal ChatId { get; set; }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * ChatId.GetHashCode();
    }
}
