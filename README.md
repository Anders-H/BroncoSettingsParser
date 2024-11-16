# Bronco Settings Parser

A self-documenting settings parser that reads `.bronco` files and returns values ​​to your program.

![Bronco logo](https://raw.githubusercontent.com/Anders-H/BroncoSettingsParser/refs/heads/main/bronco.jpg)

## Install

Broco Settings Parser requires .NET 8.0.

`// Can't do that yet...`

## Table of contents

- Load a configuration file
- [Bronco file specifications](https://github.com/Anders-H/BroncoSettingsParser/blob/main/specifications.md)
- Limitations
- Read a bronco file
- [Mapping](https://github.com/Anders-H/BroncoSettingsParser/blob/main/mapping.md)
- Validation
- [Real world examples](https://github.com/Anders-H/BroncoSettingsParser/blob/main/realworldexamples.md)

## Load a configuration file

This code loads settings from a string constant:


```
using BroncoSettingsParser;

const string settings = @"
<<<Begin:Setting:BackgroundColor>>>
    #DDDDDD
<<<End:Setting>>>

<<<Begin:Setting:ForegroundColor>>>
    #220022
<<<End:Setting>>>
";

var parser = new Parser(settings);
var response = parser.Parse();
Console.WriteLine(response.Settings.GetValue("BackgroundColor")); // #DDDDDD
Console.WriteLine(response.Settings.GetValue("ForegroundColor")); // #220022
```

This code loads settings from a `.bronco` file called `mysettings.bronco`, located in the same folder as the running exe-file.
The contents of the file is the same as the contents of the string constant in the previous example.

```
var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "mysettings.bronco")));
var response = parser.Parse();
Console.WriteLine(response.Settings.GetValue("BackgroundColor")); // #DDDDDD
Console.WriteLine(response.Settings.GetValue("ForegroundColor")); // #220022
```

The only supported datatype is string. Constrains can be implemented using other libraries.

## Bronco file specifications

A Bronco settings file is any UTF-8 encoded text file with `.bronco` as file ending. Bronco settings files are not case sensitive.

The file consists of any number of `Setting` sections. A setting opens with a row that contains the words Begin, Setting, and a name.
A name starts with a letter and consist of letters and numbers. No other characters are allowed in the name.

A setting closes with `<<<End:Setting>>>`. Whitespaces are trimmed away, but the opening row and closing row cannot have any
other text in them. This is ok:


```
<<<Begin:Setting:Name goes here>>>
<<<End:Setting>>>
```

This is ok:


```
           <<<Begin:Setting:Name goes here>>>
                              <<<End:Setting>>>
```

This is *not* ok:


```
Open a block: <<<Begin:Setting:Name goes here>>>
<<<End:Setting>>>
```

This is *not* ok:


```
<<<Begin:Setting:Name goes here>>>
<<<End:Setting>>> I have closed a block!
```

Whitespaces in names are not preserved. Whitespaces before and after the name will be removed, any whitespaces within the name (*spaces* or *tabs*) will be replaced with one space.


### Values

The value of a setting is anyting between the opening and closing tag, that is not a remark. The value of the setting `MySetting` is `I am value`:

```
<<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>>
```

Whitespaces are not preserved in values. Whitespaces in values are treated in a smular way that it is treated in HTML - the occurrence of any whitespace represents a whitespace. A whitespace in a value can be *space*, *tab* or *carriage return/line feed*.

Empty values looks like this:

```
<<<Begin:Setting:MySetting>>>
<<<End:Setting>>>
```

This is incorrect, because the block opener and the block closer must be alone on a row:

```
<<<Begin:Setting:MySetting>>><<<End:Setting>>>
```

### Remarks

Remarks are ignored, and they can appera anywhere. Comments are surrounded by `/*` and `*/`. The value of a setting is anyting between the opening and closing tag, that is not a remark. The value of the setting `MySetting` is still `I am value`:

```
<<<Begin:Setting:MySetting>>>
/* Here comes the value: */
I /* Hello */ am /* World! */ value
/* That's it */
<<<End:Setting>>>
```

If a remark appears outside a setting, the surrounding `/*` and `*/` is optional.

```
/* This is a remark */

This is also a remark.

<<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>>

And this is a remark, even without /* and */.
```

Remarks on the same line as an opening och closing row, must be surrounded by `/*` and `*/`.

## Limitations

- Escape sequences are not supported. Therefore a a name or a value of a setting cannot contain `<<<`, `>>>`, `/*` or `*/`.
- Opening tags (`<<<Begin:Setting:Example setting>>>`) and closing tags (`<<<End:Setting>>>`) must stand alone on a row in the settings file.

## Read a bronco file

This is the sample file:


```
<<<Begin:Setting:Setting 1>>>

    /* The first setting */
    I am value!

<<<End:Setting>>>
<<<Begin:Setting:The Second Setting>>>

    /* The 2:nd setting */
    I am also
    value.

<<<End:Setting>>>
```

This code iterates through the two settings:

```
using BroncoSettingsParser;

var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "samplefile.bronco")));
var response = parser.Parse();

for (var i = 0; i < response.Settings.Count; i++)
    Console.WriteLine($"Setting {i + 1}: {response.Settings[i].Value}");
```

This is the result:

```
Setting 1: I am value!
Setting 2: I am also value!
```

To read a specific setting, call the `GetValue` method:

```
using BroncoSettingsParser;

var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "samplefile.bronco")));
var response = parser.Parse();
Console.WriteLine(response.Settings.GetValue("The Second Setting"));
```

The result is `I am also value!`.

## Mapping

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
Coming soon...
```

If not all names are matched, an exception will occur.