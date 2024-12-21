namespace MyWindowsApp.BroncoUserSettingsStuff;

public class UserSettings
{
    public bool BlueBackground { get; set; }
    public bool YellowForeground { get; set; }

    public UserSettings() : this(false, false)
    {
    }

    public UserSettings(bool blueBackground, bool yellowForeground)
    {
        BlueBackground = blueBackground;
        YellowForeground = yellowForeground;
    }
}