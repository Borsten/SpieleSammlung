using Blastermind;
using Stein_Schere_Shotgun;
using Sudoku;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpieleSammlung
{
    public partial class MainWindow : Form
    {
        List<Control> WindowElements = new List<Control>();
        private Label lbl_header = new Label();
        public MainWindow()
        {
            InitializeComponent();
            CreateHub();
        }

        public void CreateHub()
        {
            this.BackColor = Color.DarkBlue;
            lbl_header = CreateLabel(lbl_header, "Spielesammlung", new Point(650, 50), new Size(300, 50), Color.LightGray, new Font("Pristina", 30), ContentAlignment.TopCenter);
            Button blastermind = new Button();
            blastermind = CreateButton(blastermind, 50, 150, 200, 40, "Blastermind", Color.LightGray, "Pristina", 20, "Start Blastermind", Blastermind_Click);
            Button stein_schere_shotgun = new Button();
            stein_schere_shotgun = CreateButton(stein_schere_shotgun, 50, 200, 200, 40, "Stein Schere Papier", Color.LightGray, "Pristina", 20, "Start Stein Schere Papier", Stein_Schere_Shotgun_Click);
            Button sudoku = new Button();
            stein_schere_shotgun = CreateButton(sudoku, 50, 250, 200, 40, "Sudoku", Color.LightGray, "Pristina", 20, "Sudoku", Sudoku_Click);
            this.Controls.Add(lbl_header);
            this.Controls.Add(blastermind);
            this.Controls.Add(stein_schere_shotgun);
        }

        public void Stein_Schere_Shotgun_Click(object sender, EventArgs e)
        {
            RemoveControls();
            app_SSS app_SSS = new app_SSS();
            app_SSS.Run(this, lbl_header);
        }

        public void Blastermind_Click(object sender, EventArgs e)
        {
            RemoveControls();
            app_blastermind blastermind = new app_blastermind();
            blastermind.Run(this, lbl_header);
        }

        public void Sudoku_Click(object sender, EventArgs e)
        {
            RemoveControls();
            app_sudoku sudoku = new app_sudoku();
            sudoku.Run(this, lbl_header);
        }

        private Button CreateButton(Button button, int locationX, int locationY, int sizeX, int sizeY, string text, Color color, string font, int fontsize,
            string tt, EventHandler click)
        {
            button.Location = new Point(locationX, locationY);
            button.Size = new Size(sizeX, sizeY);
            button.Text = text;
            button.BackColor = color;
            button.Font = new Font(font, fontsize);
            button.Click += click;
            CreateTooltip(button, tt);
            this.Controls.Add(button);
            return button;
        }

        private Label CreateLabel(Label label, string text, Point location, Size size, Color color, Font font, ContentAlignment textalign)
        {
            label.Location = location;
            label.Size = size;
            label.Text = text;
            label.BackColor = color;
            label.Font = font;
            label.TextAlign = textalign;
            return label;
        }

        public void CreateTooltip(Control Box, string message)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(Box, message);
        }

        private void RemoveControls()
        { this.Controls.Clear(); }
    }
}