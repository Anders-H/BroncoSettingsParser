namespace BroncoSettingsParser.Comments;

public class Remover
{
    private readonly string _comment;
    private bool _commentScope;

    public Remover(string comment)
    {
        _comment = comment;
        _commentScope = false;
    }

    public string Remove(out bool commentScope)
    {
        var result = _comment;

        do
        {
            var firstOpen = result.IndexOf("/*", StringComparison.Ordinal);
            var firstClose = result.IndexOf("*/", StringComparison.Ordinal);

            if (firstOpen < 0 && firstClose < 0)
            {
                commentScope = _commentScope;
                return result;
            }
            
            if (firstOpen >= 0 && firstClose >= 0)
            {
                if (firstClose > firstOpen)
                {
                    result = Cut(result, firstOpen, firstClose).Trim();
                    _commentScope = false;
                    commentScope = false;
                }
                else
                {
                    result = CutBeginning(result, firstClose).Trim();
                    firstOpen = result.LastIndexOf("/*", StringComparison.Ordinal);
                    result = CutEnd(result, firstOpen);
                    _commentScope = true;
                    commentScope = true;
                }
            }
            else if (firstOpen >= 0)
            {
                result = CutEnd(result, firstOpen);
                _commentScope = true;
                commentScope = true;
            }
            else if (firstClose >= 0)
            {
                result = CutBeginning(result, firstClose);
                _commentScope = false;
                commentScope = false;
            }
            else
            {
                throw new SystemException("Comment parse error.");
            }

        } while (true);
    }

    private static string Cut(string s, int start, int end)
    {
        var result = "";

        if (start > 0)
            result += s.Substring(0, start);

        result += s[(end + 2)..];
        return result;
    }

    public static string CutBeginning(string s, int end)
    {
        return s[(end + 2)..];
    }

    public static string CutEnd(string s, int start)
    {
        return s[..start];
    }
}