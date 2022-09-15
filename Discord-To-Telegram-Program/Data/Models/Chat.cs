using System.Text.Json.Serialization;

namespace DiscordToTelegram.Data.Models;

public class Chat
{
    [JsonConstructor]
    public Chat(decimal id, string name) => (Id, Name) = (id, name);

    public Chat(decimal id) => Id = id;

    public decimal Id { get; set; }
    public string? Name { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var compared = (Chat)obj;

        return compared.Id == this.Id;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public override string ToString()
    {
        return (this.Name + ": " + this.Id);
    }
}
