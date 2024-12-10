# Bronco file specifications

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

This is ok:

```
    /* Open a block: */   <<<Begin:Setting:Name goes here>>>
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

## Values

The value of a setting is anyting between the opening and closing tag, that is not a remark. The value of the setting `MySetting` is `I am value`:

```
<<<Begin:Setting:MySetting>>>
I am value
<<<End:Setting>>>
```

Whitespaces are not preserved in values. Whitespaces in values are treated in a smular way that it is treated in HTML - the occurrence of any whitespace represents a whitespace. A whitespace in a value can be *space*, *tab* or *carriage return/line feed*.

## Remarks

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

---

[Back](https://github.com/Anders-H/BroncoSettingsParser/blob/main/README.md)