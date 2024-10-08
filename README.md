# Bronco Settings Parser

A self-documenting settings parser that reads .bronco files and returns values ​​to your program.

## Load a configuration file

TODO

## Bronco file specification

A Bronco settings file is any UTF-8 encoded text file with .bronco as file ending. Bronco settings file are note case sensitive.

The file consist of any number of `Setting` sections. A setting opens with a row that contains the words Begin, Setting, and a name.
A name starts with a letter and consist of letters and numbers. No other characters are allowed in the name.

A setting closes with `<<<End:Setting)>>>`. Whitespaces are trimmed away, but the opening row and closing row cannot have any
other text in them. This is ok:


```
<<<Begin:Setting:NameGoesHere)>>>
<<<End:Setting)>>>
```

This is ok:


```
           <<<Begin:Setting:NameGoesHere)>>>
                                   <<<End:Setting)>>>
```

This is not ok:


```
Open a block: <<<Begin:Setting:NameGoesHere)>>>
<<<End:Setting)>>>
```

This is not ok:


```
<<<Begin:Setting:NameGoesHere)>>>
<<<End:Setting)>>> I have closed a block!
```

### Values

### Remarks

## Read a bronco file

This is the sample file:


```
<<<Begin:Setting:NameGoesHere)>>>
<<<End:Setting)>>>
```

And this is the sample code:
