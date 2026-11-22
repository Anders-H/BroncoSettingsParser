using BroncoSettingsParser.Exceptions;
using BroncoSettingsParser.ValueParsers;
using System.Reflection;

namespace BroncoSettingsParser.ResponseModel;

public class ParseResult
{
    private readonly ValueParserList _valueParsers;
    public Status Status { get; }
    public string Message { get; }
    public SettingCollection Settings { get; }

    internal ParseResult(Status status, string message, SettingCollection settings)
    {
        _valueParsers = new ValueParserList();
        Status = status;
        Message = message;
        Settings = settings;
    }

    public void SetValueParser(IValueParser valueParser)
    {
        _valueParsers.SetValueParser(valueParser);
    }

    public T Map<T>() where T : new()
    {
        object result = new T();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var propertyInfo in properties)
        {
            if (propertyInfo.SetMethod?.IsPublic != true || propertyInfo.GetIndexParameters().Length != 0)
                continue;

            var propertyName = propertyInfo.Name;

            if (!Settings.HasSetting(propertyName))
                throw new PropertyMissingException($"Property is missing: {propertyName}");

            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(result, Settings.GetValue(propertyName));
            }
            else
            {
                var vp = _valueParsers.GetParser(propertyInfo.PropertyType.FullName ?? "");
                var stringValue = Settings.GetValue(propertyName);
                var typedValue = vp.Parse(stringValue);
                propertyInfo.SetValue(result, typedValue);
            }
        }

        return (T)result;
    }

    public T? TryGetValue<T>(string settingName)
    {
        var v = Settings.TryGetValue(settingName);

        if (v == null)
            return default;

        if (typeof(T) == typeof(string))
            return (T)(object)v;

        var typeName = typeof(T).FullName ?? "";

        try
        {
            var vp = _valueParsers.GetParser(typeName);
            var typedValue = vp.Parse(v);
            return (T)typedValue;
        }
        catch
        {
            return default;
        }
    }
}