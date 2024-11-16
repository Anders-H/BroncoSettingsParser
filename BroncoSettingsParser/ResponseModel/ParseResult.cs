namespace BroncoSettingsParser.ResponseModel;

public class ParseResult
{
    public Status Status { get; }
    public string Message { get; }
    public SettingCollection Settings { get; }

    internal ParseResult(Status status, string message, SettingCollection settings)
    {
        Status = status;
        Message = message;
        Settings = settings;
    }

    public T Map<T>() where T : class, new()
    {

    }
}