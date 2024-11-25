namespace BroncoSettingsParser.ValueParsers;

public interface IValueParser<out T>
{
    T Parse(string source);
    Type GetType(Type type) => typeof(T);
}