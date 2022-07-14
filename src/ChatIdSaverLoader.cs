using System.Text.Json.Serialization;
using System.Text.Json;

public static class ChatIdSaverLoader
{

    private const string ChatsPath = "./chats.json";

    public static void Save(List<long> chats)
    {
        var serializationOptions = new JsonSerializerOptions {
            WriteIndented = true
        };
        var jsonString = JsonSerializer.Serialize(chats, serializationOptions);   
        File.WriteAllText(ChatsPath, jsonString);
    }

    public static List<long> Load()
    {
        if(new FileInfo(ChatsPath).Length < 6)
            return new List<long>();

        try  
        {
            return JsonSerializer.Deserialize<List<long>>(File.ReadAllText(ChatsPath));    
        }
        catch(Exception e)
        {
            return new List<long>();
        }

    }

}