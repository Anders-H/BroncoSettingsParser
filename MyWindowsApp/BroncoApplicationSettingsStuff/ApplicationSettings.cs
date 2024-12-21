namespace MyWindowsApp.BroncoApplicationSettingsStuff;

public class ApplicationSettings
{
    public string WelcomeMessage { get; set; }
    public Rectangle MyRectangle { get; set; }

    public ApplicationSettings() : this("", Rectangle.Empty)
    {
    }

    public ApplicationSettings(string welcomeMessage, Rectangle myRectangle)
    {
        WelcomeMessage = welcomeMessage;
        MyRectangle = myRectangle;
    }
}