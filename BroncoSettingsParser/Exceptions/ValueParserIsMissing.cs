namespace BroncoSettingsParser.Exceptions;

public class ValueParserIsMissing : SystemException
{
    public ValueParserIsMissing(string message) : base(message)
    {
    }
}