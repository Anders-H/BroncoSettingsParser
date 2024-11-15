using BroncoSettingsParser;

var parser = new Parser(new FileInfo(Path.Combine()));
var response = parser.Parse();
Console.WriteLine(response.Settings.GetValue("BackgroundColor")); // #DDDDDD
Console.WriteLine(response.Settings.GetValue("ForegroundColor")); // #220022