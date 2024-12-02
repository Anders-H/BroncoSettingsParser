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

    public IValueParser GetParser<T>(T type)
    {
        foreach (var p in _parsers)
        {
            if (p.CanParseToType(type!.GetType()))
                return p;
        }

        throw new ValueParserIsMissing(typeof(T).FullName ?? "Unknown");
    }
}