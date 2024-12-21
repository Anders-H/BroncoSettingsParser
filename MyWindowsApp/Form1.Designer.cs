namespace MyWindowsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            blueBackgroundToolStripMenuItem = new ToolStripMenuItem();
            yellowForegroundToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(737, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { blueBackgroundToolStripMenuItem, yellowForegroundToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // blueBackgroundToolStripMenuItem
            // 
            blueBackgroundToolStripMenuItem.Name = "blueBackgroundToolStripMenuItem";
            blueBackgroundToolStripMenuItem.Size = new Size(171, 22);
            blueBackgroundToolStripMenuItem.Text = "Blue background";
            blueBackgroundToolStripMenuItem.Click += blueBackgroundToolStripMenuItem_Click;
            // 
            // yellowForegroundToolStripMenuItem
            // 
            yellowForegroundToolStripMenuItem.Name = "yellowForegroundToolStripMenuItem";
            yellowForegroundToolStripMenuItem.Size = new Size(171, 22);
            yellowForegroundToolStripMenuItem.Text = "Yellow foreground";
            yellowForegroundToolStripMenuItem.Click += yellowForegroundToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(737, 522);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            Paint += Form1_Paint;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem blueBackgroundToolStripMenuItem;
        private ToolStripMenuItem yellowForegroundToolStripMenuItem;
    }
}
