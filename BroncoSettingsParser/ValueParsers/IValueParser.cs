namespace BroncoSettingsParser.ValueParsers;

public interface IValueParser
{
    bool CanParseToType(Type type);
    object Parse(string source);
}