﻿namespace BroncoSettingsParser.ResponseModel;

public class Setting
{
    public Setting(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public string Value { get; }
}