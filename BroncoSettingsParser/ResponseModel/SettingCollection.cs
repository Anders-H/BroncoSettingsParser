namespace BroncoSettingsParser.ResponseModel;

public class SettingCollection
{
    private readonly Dictionary<string, string> _settings;

    public SettingCollection()
    {
        _settings = new Dictionary<string, string>();
    }

    public int Count =>
        _settings.Count;

    public Setting? GetSetting(string key) =>
        _settings.TryGetValue(key, out var value) ? new Setting(key, value) : null;

    public string? TryGetValue(string key) =>
        _settings.GetValueOrDefault(key);

    public string GetValue(string key) =>
        _settings[key];

    internal bool Set(Setting setting) =>
        _settings.TryAdd(setting.Name, setting.Value);

    internal void SetOrFail(Setting setting)
    {
        if (!Set(setting))
            throw new ArgumentException($"Setting exists: {setting.Name}");
    }

    public Setting this[int index]
    {
        get
        {
            var item = _settings.ElementAt(index);
            return new Setting(item.Key, item.Value);
        }
    }
}