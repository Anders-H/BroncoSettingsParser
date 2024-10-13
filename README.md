# Bronco Settings Parser

A self-documenting settings parser that reads `.bronco` files and returns values ​​to your program.

![Bronco logo](https://raw.githubusercontent.com/Anders-H/BroncoSettingsParser/refs/heads/main/bronco.jpg)

## Load a configuration file

This code loads settings from a string constant:


```

```

This code loads settings from a `.bronco` file called `MySettings.bronco`, located in the same folder as the running exe-file:


```

```



The only supported datatype is string. Constrains can be implemented using other libraries.

## Bronco file specification

A Bronco settings file is any UTF-8 encoded text file with `.bronco` as file ending. Bronco settings files are not case sensitive.

The file consists of any number of `Setting` sections. A setting opens with a row that contains the words Begin, Setting, and a name.
A name starts with a letter and consist of letters and numbers. No other characters are allowed in the name.

A setting closes with `<<<End:Setting>>>`. Whitespaces are trimmed away, but the opening row and closing row cannot have any
other text in them. This is ok:


```
<<<Begin:Setting:NameGoesHere>>>
<<<End:Setting>>>
```

This is ok:


```
           <<<Begin:Setting:NameGoesHere>>>
                              <<<End:Setting>>>
```

This is *not* ok:


```
Open a block: <<<Begin:Setting:NameGoesHere>>>
<<<End:Setting>>>
```

This is *not* ok:


```
<<<Begin:Setting:NameGoesHere>>>
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

And this is a remark, even without /* and /*.
```

## Limitations

* Escape sequences are not supported. Therefore a a name or a value of a setting cannot contain `<<<`, `>>>`, `/*` or `*/`.
* Opening tags (`<<<Begin:Setting:Example setting>>>`) and closing tags (`<<<End:Setting>>>`) must stand alone on a row in the settings file.


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

The test method `CanParseBasicBroncoFile3` reads the file above.


```
```


This is the result:

```
Setting 1: I am value!
Setting 2: I am also value!
```

To read a specific setting:

```
```

The result is `I am also value!`.