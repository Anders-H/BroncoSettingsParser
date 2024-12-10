using System.Diagnostics.Contracts;
using BroncoSettingsParser;

namespace MyWindowsApp
{
    public partial class Form1 : Form
    {
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
            var settings = raw.Map<>()
        }
    }
}
