namespace BroncoSettingsParser.ResponseModel;

public class ParseResult
{
    public Status Status { get; }
    public string Message { get; }
    public SettingCollection Settings { get; }
}