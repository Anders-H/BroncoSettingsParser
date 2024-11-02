namespace BroncoSettingsParser.Comments;

public class Remover
{
    private readonly string _comment;

    public Remover(string comment)
    {
        _comment = comment;
    }

    public string Remove()
    {
        var result = _comment;

        do
        {
            var firstOpen = _comment.IndexOf("/*", StringComparison.Ordinal);
            var firstClose = _comment.IndexOf("*/", StringComparison.Ordinal);

            if (firstOpen < 0 && firstClose < 0)
                return result;

        } while (true);

        return "";
    }
}