using System.Reflection;

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

            switch (propertyInfo.PropertyType.FullName)
            {
                case "System.String":
                    propertyInfo.SetValue(result, Settings.GetValue(propertyInfo.Name));
                    break;
                default:
                    throw new SystemException($"Property type not supported: {propertyInfo.PropertyType.FullName}");
            }
        }

        return result;
    }
}