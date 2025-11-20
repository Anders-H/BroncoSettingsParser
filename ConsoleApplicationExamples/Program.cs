using BroncoSettingsParser;
using BroncoSettingsParser.ResponseModel;

// Load the settings file.
var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "mapping.bronco")));

// Parse the settings file.
var response = parser.Parse();

// Enable extended value parsing.
response.SetValueParser(new BroncoSettingsParser.ValueParsers.DefaultParsers.CommaSeparatedStringListParser());

// Check for errors.
if (response.Status != Status.Success)
    throw new SystemException("Parse failed.");

// Map the settings to a nice strongly typed object.
var settings = response.Map<MyNiceSettings>();

// Print out the results.
Console.WriteLine(settings.Setting1);
Console.WriteLine(settings.TheSecondSetting);
Console.WriteLine(string.Join(" --- ", settings.MyCommaSeparatedList));

public class MyNiceSettings
{
    public string Setting1 { get; set; }
    public string TheSecondSetting { get; set; }
    public string[] MyCommaSeparatedList { get; set; }

    public MyNiceSettings() : this("", "", [])
    {
    }

    public MyNiceSettings(string setting1, string theSecondSetting, string[] myCommaSeparatedList)
    {
        Setting1 = setting1;
        TheSecondSetting = theSecondSetting;
        MyCommaSeparatedList = myCommaSeparatedList;
    }
}