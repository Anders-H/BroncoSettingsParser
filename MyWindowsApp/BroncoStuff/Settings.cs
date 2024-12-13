namespace MyWindowsApp.BroncoStuff;

public class Settings
{
    public string WelcomeMessage { get; set; }
    public Rectangle MyRectangle { get; set; }

    public Settings() : this("", Rectangle.Empty)
    {
    }

    public Settings(string welcomeMessage, Rectangle myRectangle)
    {
        WelcomeMessage = welcomeMessage;
        MyRectangle = myRectangle;
    }
}