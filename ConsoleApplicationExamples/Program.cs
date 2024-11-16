using BroncoSettingsParser;
using BroncoSettingsParser.ResponseModel;

var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "mapping.bronco")));
var response = parser.Parse();

if (response.Status != Status.Success)
    throw new SystemException("Parse failed.");

var settings = response.Map<MyNiceSettings>();
Console.WriteLine(settings.Setting1);
Console.WriteLine(settings.TheSecondSetting);

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