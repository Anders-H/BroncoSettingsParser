namespace BroncoSettingsParser.ValueParsers;

public interface IValueParser
{
    bool CanParseToType(string fullName);
    object Parse(string source);
}