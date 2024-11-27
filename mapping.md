# Mapping

Mapping requires the least code to acquire settings from a `.bronco` file.
All settings need to be named accordingly to C# name rules.


```
<<<Begin:Setting:Setting1>>>

    /* The first setting */
    I am value!

<<<End:Setting>>>
<<<Begin:Setting:TheSecondSetting>>>

    /* The 2:nd setting */
    I am also
    value.

<<<End:Setting>>>
```

Class to map the settings to:

```
public class MyNiceSettings
{
    public string Setting1 { get; set; }
    public string TheSecondSetting { get; set; }

    public MyNiceSettings() : this("", "")
    {
    }

    public MyNiceSettings(string setting1, string theSecondSetting)
    {
        Setting1 = setting1;
        TheSecondSetting = theSecondSetting;
    }
}
```

Read the settings:

```
using BroncoSettingsParser;
using BroncoSettingsParser.ResponseModel;

var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "mapping.bronco")));
var response = parser.Parse();

if (response.Status != Status.Success)
    throw new SystemException("Parse failed.");

var settings = response.Map<MyNiceSettings>();
Console.WriteLine(settings.Setting1);
Console.WriteLine(settings.TheSecondSetting);
```

If not all names are matched, an exception will occur.

## Datatypes

All target properties are expected to be strings.
If a property has a datatype other than `string`, a value parser will be invoked.
For each datatype you want to support,
you need to create a value parser and add it to the `ParseResult` object before calling the `Map` function.
If a value parser isn't provided, an exception will occur.

This example has a `string` setting (supported by default), an `int` setting and a `Point` setting.

```
<<<Begin:Setting:MyStringSetting>>>
    Hello!
<<<End:Setting>>>
<<<Begin:Setting:MyIntSetting>>>
    56
<<<End:Setting>>>
<<<Begin:Setting:MyPointSetting>>>
    45,81
<<<End:Setting>>>
```

This source will be passed as a string to the parser,
and it will be mapped to an instance of this class:

```
public class DataTypeSupport
{
    public string MyStringSetting { get; set; }
    public int MyIntSetting { get; set; }
    public Point MyPointSetting { get; set; }

    public DataTypeSupport() : this("", 0, Point.Empty)
    {
    }

    public DataTypeSupport(string myStringSetting, int myIntSetting, Point myPointSetting)
    {
        MyStringSetting = myStringSetting;
        MyIntSetting = myIntSetting;
        MyPointSetting = myPointSetting;
    }
}
```

All custom value parsers implement the IValueParser&lt;T&gt;. This is the `int` parser:

```
public class IntParser : IValueParser<int>
{
    public int Parse(string source) =>
        int.Parse(source);

}
```

And this is the `Point` parser:

```
public class PointParser : IValueParser<Point>
{
    public Point Parse(string source)
    {
        var parts = source.Split(',');
        return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
```

*You must provide the value parsers yourself, and they can be implemented as you like.*

This is a working example:

```

```

---

[Back](https://github.com/Anders-H/BroncoSettingsParser/blob/main/README.md)