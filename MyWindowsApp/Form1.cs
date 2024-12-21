using BroncoSettingsParser;
using MyWindowsApp.BroncoStuff;

namespace MyWindowsApp;

public partial class Form1 : Form
{
    private Rectangle _rectangle;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
#if DEBUG
        const string filename = "mybroncosettings.Development.bronco";
#else
        const string filename = "mybroncosettings.bronco";
#endif
        var parser = Parser.LoadSettingsFromApplicationDirectory(filename);
        var raw = parser.Parse();
        raw.SetValueParser(new RectangleParser());
        var settings = raw.Map<Settings>();
        Text = settings.WelcomeMessage;
        _rectangle = settings.MyRectangle;
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(blueBackgroundToolStripMenuItem.Checked ? Color.Blue : Color.Black);
        using var p = new Pen(yellowForegroundToolStripMenuItem.Checked ? Color.Yellow: Color.Violet, 5);
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
}