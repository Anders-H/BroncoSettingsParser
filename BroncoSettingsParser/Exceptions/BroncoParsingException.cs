namespace BroncoSettingsParser.Exceptions;

public abstract class BroncoParsingException : SystemException
{
    protected BroncoParsingException(string message) : base(message)
    {
    }
}