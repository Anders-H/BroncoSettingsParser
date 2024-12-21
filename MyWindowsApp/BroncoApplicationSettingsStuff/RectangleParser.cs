using System.Globalization;
using BroncoSettingsParser.ValueParsers;

namespace MyWindowsApp.BroncoApplicationSettingsStuff;

public class RectangleParser : IValueParser
{
    public bool CanParseToType(string fullName) =>
        typeof(Rectangle).FullName == fullName;

    public object Parse(string source)
    {
        var parts = source.Split(',');
        var x = int.Parse(parts[0], NumberStyles.Any);
        var y = int.Parse(parts[1], NumberStyles.Any);
        var w = int.Parse(parts[2], NumberStyles.Any);
        var h = int.Parse(parts[3], NumberStyles.Any);
        return new Rectangle(x, y, w, y);
    }
}