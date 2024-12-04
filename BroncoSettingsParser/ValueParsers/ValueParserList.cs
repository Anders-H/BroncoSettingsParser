using BroncoSettingsParser.Exceptions;

namespace BroncoSettingsParser.ValueParsers;

public class ValueParserList
{
    private readonly List<IValueParser> _parsers;

    public ValueParserList()
    {
        _parsers = [];
    }

    public void SetValueParser(IValueParser valueParser)
    {
        _parsers.Remove(valueParser);
        _parsers.Add(valueParser);
    }

    public IValueParser GetParser(string fullName)
    {
        foreach (var p in _parsers)
        {
            if (p.CanParseToType(fullName))
                return p;
        }

        throw new ValueParserIsMissing(fullName);
    }
}