using BroncoSettingsParser.Exceptions;

namespace BroncoSettingsParser.ValueParsers;

public class ValueParserList
{
    private readonly List<IValueParser<object>> _parsers;

    public ValueParserList()
    {
        _parsers = [];
    }

    public void SetValueParser(IValueParser<object> valueParser)
    {
        _parsers.Remove(valueParser);
        _parsers.Add(valueParser);
    }

    public IValueParser<T> GetParser<T>(T type)
    {
        foreach (var valueParser in _parsers.Where(valueParser => valueParser.GetType() == typeof(T)))
            return (IValueParser<T>)valueParser;

        throw new ValueParserIsMissing(typeof(T).FullName ?? "Unknown");
    }
}