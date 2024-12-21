using BroncoSettingsParser;
using MyWindowsApp.BroncoApplicationSettingsStuff;
using MyWindowsApp.BroncoUserSettingsStuff;

namespace MyWindowsApp;

public partial class Form1 : Form
{
    private Rectangle _rectangle;
    private const string UserSettingsKey = "79739B40-70F6-4CF2-B058-8DFF78E26D35"; // Random GUID
    private const string UserSettingsName = "BroncoExample"; // Human readable filename
    private UserSettings _userSettings;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Application settings
#if DEBUG
        const string filename = "mybroncosettings.Development.bronco";
#else
        const string filename = "mybroncosettings.bronco";
#endif
        var parser = Parser.LoadSettingsFromApplicationDirectory(filename);
        var raw = parser.Parse();
        raw.SetValueParser(new RectangleParser());
        var settings = raw.Map<ApplicationSettings>();
        Text = settings.WelcomeMessage;
        _rectangle = settings.MyRectangle;

        // Load user settings
        parser = Parser.GetUserSettings(UserSettingsKey, UserSettingsName);
        raw = parser.Parse();
        _userSettings = raw.Map<UserSettings>();
        blueBackgroundToolStripMenuItem.Checked = _userSettings.BlueBackground;
        yellowForegroundToolStripMenuItem.Checked = _userSettings.YellowForeground;
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(blueBackgroundToolStripMenuItem.Checked ? Color.Blue : Color.Black);
        using var p = new Pen(yellowForegroundToolStripMenuItem.Checked ? Color.Yellow : Color.Violet, 5);
        g.DrawRectangle(p, _rectangle);
    }

    private void blueBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
    {
        blueBackgroundToolStripMenuItem.Checked = !blueBackgroundToolStripMenuItem.Checked;
        Invalidate();
    }

    private void yellowForegroundToolStripMenuItem_Click(object sender, EventArgs e)
    {
        yellowForegroundToolStripMenuItem.Checked = !yellowForegroundToolStripMenuItem.Checked;
        Invalidate();
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        // Save user settings
        _userSettings.BlueBackground = blueBackgroundToolStripMenuItem.Checked;
        yellowForegroundToolStripMenuItem.Checked = _userSettings.YellowForeground;
        Parser.SaveUserSettings(UserSettingsKey, UserSettingsName, _userSettings); // TODO: Add interface for serializability
    }
}