namespace BroncoSettingsParser.Exceptions;

public class PropertyTypeNotSupportedException : BroncoParsingException
{
    public PropertyTypeNotSupportedException(string message) : base(message)
    {
    }
}