namespace BroncoSettingsParser.ResponseModel;

public class SettingCollection
{
    private Dictionary<string, string> _settings;

    public SettingCollection()
    {
        _settings = new Dictionary<string, string>();
    }

    public int Count =>
        _settings.Count;
}