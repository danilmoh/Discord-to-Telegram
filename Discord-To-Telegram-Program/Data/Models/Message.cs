namespace DiscordToTelegram.Data.Models;
public record Message
{
    public decimal Id { get; set; }
    public decimal ChatId { get; set; }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode() * ChatId.GetHashCode();
    }
}
