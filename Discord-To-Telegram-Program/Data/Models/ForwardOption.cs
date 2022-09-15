using System.Text;

namespace DiscordToTelegram.Data.Models;

public record ForwardOption
{
    public List<string> Sources { get; } = new List<string>();
    public List<string> Destinations { get; } = new List<string>();

    public ForwardOption(List<string> src, List<string> dest) => (Sources, Destinations) = (src, dest);

    public override string ToString()
    {
        var resultString = new StringBuilder();
        resultString.Append('(');

        for (int i = 0; i < this.Sources.Count; i++)
        {
            resultString.Append(this.Sources[i]);

            if (i == this.Sources.Count - 1)
            {
                resultString.Append(')');
                break;
            }
            else
                resultString.Append(',');
        }

        resultString.Append('(');

        for (int i = 0; i < this.Destinations.Count; i++)
        {

            resultString.Append(this.Destinations[i]);

            if (i == this.Destinations.Count - 1)
            {
                resultString.Append(')');
                break;
            }
            else
                resultString.Append(',');
        }


        return resultString.ToString();
    }
}
