namespace BroncoSettingsParser;

public static class Tools
{
    public static DirectoryInfo ExeFolder =>
        new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory!;
}