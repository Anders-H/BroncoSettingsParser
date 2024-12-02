using BroncoSettingsParser.Exceptions;
using BroncoSettingsParser.ValueParsers;

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
        var result = new T();
        var properties = result.GetType().GetProperties();

        foreach (var propertyInfo in properties)
        {
            if (!propertyInfo.CanWrite)
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
                var vp = _valueParsers.GetParser(typeof(int)); //(propertyInfo.PropertyType);
                var stringValue = Settings.GetValue(propertyName);
                var typedValue = vp.Parse(stringValue);
                propertyInfo.SetValue(result, typedValue);
            }
        }

        return result;
    }
}