namespace BroncoSettingsParser;

public static class Tools
{
    public static DirectoryInfo ExeFolder =>
        new DirectoryInfo(AppContext.BaseDirectory);
}