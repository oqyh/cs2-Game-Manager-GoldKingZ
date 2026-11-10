using System.Globalization;
namespace Game_Manager_GoldKingZ;
public sealed class CountdownText
{
    private int second = int.MinValue;
    private string? key, culture, text;
    public string Get(double remaining, string messageKey, string language, Func<string, int, string> format)
    {
        int next = (int)Math.Ceiling(Math.Max(0, remaining));
        if (next != second || key != messageKey || culture != language || text == null)
        {
            text = format(messageKey, next);
            second = next; key = messageKey; culture = language;
        }
        return text;
    }
}
