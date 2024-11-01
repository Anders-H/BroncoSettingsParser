namespace BroncoSettingsParser.ResponseModel;

public class Setting
{
    internal Setting(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public string Value { get; internal set; }
}