using BroncoSettingsParser.Exceptions;

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
        var result = new T();
        var properties = result.GetType().GetProperties();

        foreach (var propertyInfo in properties)
        {
            if (!propertyInfo.CanWrite)
                continue;

            var propertyName = propertyInfo.Name;

            if (!Settings.HasSetting(propertyName))
                throw new PropertyMissingException($"Property is missing: {propertyName}");

            switch (propertyInfo.PropertyType.FullName)
            {
                case "System.String":
                    propertyInfo.SetValue(result, Settings.GetValue(propertyName));
                    break;
                default:
                    throw new PropertyTypeNotSupportedException($"Property type not supported: {propertyInfo.PropertyType.FullName}");
            }
        }

        return result;
    }
}