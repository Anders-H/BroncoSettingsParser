using BroncoSettingsParser;

var parser = new Parser(new FileInfo(Path.Combine(Tools.ExeFolder.FullName, "samplefile.bronco")));
var response = parser.Parse();
Console.WriteLine(response.Settings.GetValue("The Second Setting"));