
namespace DiscordToTelegram.Exceptions;
public class WrongInputException : System.Exception
{
    public WrongInputException() { }
    public WrongInputException(string message) : base(message) { }
    public WrongInputException(string message, System.Exception inner) : base(message, inner) { }
    protected WrongInputException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
