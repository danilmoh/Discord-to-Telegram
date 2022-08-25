

namespace DiscordToTelegram.Exceptions;
public class EmptyInputException : System.Exception
{

    public EmptyInputException() { }
    public EmptyInputException(string message) : base(message) { }
    public EmptyInputException(string message, System.Exception inner) : base(message, inner) { }
    protected EmptyInputException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

}
