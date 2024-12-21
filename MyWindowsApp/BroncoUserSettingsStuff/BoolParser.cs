using BroncoSettingsParser.ValueParsers;

namespace MyWindowsApp.BroncoUserSettingsStuff;

public class BoolParser : IValueParser
{
    public bool CanParseToType(string fullName) =>
        typeof(bool).FullName == fullName;

    public object Parse(string source) =>
        source.ToLower() == "true";
}