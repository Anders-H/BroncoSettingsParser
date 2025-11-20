namespace BroncoSettingsParser.ValueParsers.DefaultParsers;

public class CommaSeparatedStringListParser : IValueParser
{
    public bool CanParseToType(string fullName) =>
        fullName == "System.String[]";

    public object Parse(string source)
    {
        var result = new List<string>();
        var s = source.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var s2 in s)
        {
            var s3 = s2.Trim();

            if (!string.IsNullOrEmpty(s3))
                result.Add(s3);
        }

        return result.ToArray();
    }
}