namespace BroncoSettingsParser.Exceptions;

public class PropertyMissingException : BroncoParsingException
{
    public PropertyMissingException(string message) : base(message)
    {
    }
}