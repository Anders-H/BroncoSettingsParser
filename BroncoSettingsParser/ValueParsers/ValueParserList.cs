using BroncoSettingsParser.Exceptions;

namespace BroncoSettingsParser.ValueParsers;

public class ValueParserList<T>
{
    private readonly List<IValueParser<T>> _parsers;

    public ValueParserList()
    {
        _parsers = [];
    }

    public void SetValueParser(IValueParser<T> valueParser)
    {
        _parsers.Remove(valueParser);
        _parsers.Add(valueParser);
    }

    public IValueParser<T> GetParser(Type type)
    {
        foreach (var valueParser in _parsers.Where(valueParser => valueParser.GetType() == type))
            return valueParser;

        throw new ValueParserIsMissing(typeof(T).FullName ?? "Unknown");
    }
}