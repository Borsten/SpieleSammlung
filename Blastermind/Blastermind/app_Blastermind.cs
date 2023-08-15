using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Blastermind
{
    public partial class app_blastermind : Form
    {
        private int counter = 0;
        private List<ComboBox> InputBoxes = new List<ComboBox>();
        private string[][] ComboboxContents =
        {
            new string[] { "Red", "Blue", "Green", "Yellow", "Brown", "Orange" },
            new string[] {"0", "1", "2", "3", "4", "5"}
        };
        private string[] code = new string[2];
        public Form Game { get; set; }
        private Label lbl_header = new Label();
        private Button btn_generate = new Button(); 
        Font f = new Font("Pristina", 15);
        public app_blastermind()
        {
            InitializeComponent();
        }

        public void Run(Form Game, Label lbl_header)
        {
            this.lbl_header = lbl_header;
            this.Game = Game;
            CreateUI();
            code = GenerateCode(code);
        }

        private void CreateUI()
        {
            lbl_header.Text = "MasterMind";
            Game.Controls.Add(lbl_header);
            btn_generate = CreateButton(btn_generate, "Check", 1200, 700, 300, 100, Color.White, new Font("Pristina", 40, FontStyle.Bold), btn_generate_Click);
            CreateRow();
        }

        private void CreateRow()
        {
            InputBoxes.Clear();
            CreateLabel("lbl_row" + counter.ToString(), counter.ToString(), 100, 200 + counter * 40, 50, 30, Color.DarkBlue, new Font("Pristina", 24, FontStyle.Bold));
            for (int i = 0; i < 4; i++)
            {
                CreateCombobox("cbo_input" + i.ToString(), 200 + i * 250, 200 + counter * 40, 200, 30,new Font("Pristina", 15), "Gib einen Wert zwischen einschliesslich 0 und 5 ein."); 
            }
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            bool retry = false;
            retry = Check(code[1], ComboboxContents[0]);
            if (!retry)
            {
                counter++;
                if (counter == 12)
                    EndGame(false);
                foreach (ComboBox oldBox in InputBoxes)
                    oldBox.Enabled = false;
                CreateRow();
            }
        }

        private string[] GenerateCode(string[] genCode)
        {
            Random rnd = new Random();
            for (int i = 0; i <= 3; i++)
            { int number = rnd.Next(0, 6); genCode[0] += ComboboxContents[0][number]; genCode[1] += ComboboxContents[1][number]; }
            Label result1 = new Label();
            Label result2 = new Label();
            result1.Text = genCode[0];
            result2.Text = genCode[1];
            result1.Location = new Point(1200, 30);
            result2.Location = new Point(1200, 70);
            result1.BackColor = Color.White;
            result2.BackColor = Color.White;
            Game.Controls.Add(result1);
            Game.Controls.Add(result2);
            return genCode;
        }

        private void CreateCombobox(string name, int locationX, int locationY, int sizeX, int sizeY, Font font, string tt)
        {
            ComboBox inputBox = new ComboBox();
            inputBox.Name = name;
            inputBox.Location = new Point(locationX, locationY);
            inputBox.Size = new Size(sizeX, sizeY);
            inputBox.Font = font;
            inputBox.DrawMode = DrawMode.OwnerDrawVariable;
            inputBox.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            inputBox.Items.AddRange(ComboboxContents[0]);
            CreateTooltip(inputBox, tt);
            Game.Controls.Add(inputBox);
            InputBoxes.Add(inputBox);
        }

        private bool Check(string checkCode, string[] inputs)
        {
            string tempCode = string.Empty;
            int picCounter = 0;
            int tempCounter;
            bool retryInput = false;
            

            foreach (ComboBox inputBox in InputBoxes)
            {
                tempCounter = 0;
                foreach (string s in inputs)
                {
                    if (s.Equals(inputBox.Text))
                    { tempCode = tempCode + ComboboxContents[1][tempCounter]; break; }
                    else
                        tempCounter++;
                    if (tempCounter == 6)
                        retryInput = true;
                }
            }
            if (retryInput)
            {
                MessageBox.Show("Falsche Werte, oder Feld leer", "Eingabe falsch", MessageBoxButtons.OK);
                return retryInput;
            }

            if (tempCode.Equals(checkCode))
                EndGame(true);
            for (int i = 0; i < checkCode.Length; ++i)
            {
                if (checkCode[i].Equals(tempCode[i]))
                {
                    checkCode = checkCode.Remove(i, 1);
                    tempCode = tempCode.Remove(i, 1);
                    CreatePicturebox("pbo_Check" + picCounter.ToString(), Color.Black, 1200, 200, picCounter, "Die eingegebene Zahl hat den richtigen Wert und die richtige Position");
                    picCounter++;
                    i--;
                }
            }

            for (int i = 0; i < tempCode.Length; ++i)
            {
                for (int j = 0; j < checkCode.Length; ++j)
                {
                    if (tempCode[i].Equals(checkCode[j]))
                    {
                        checkCode = checkCode.Remove(j, 1);
                        tempCode = tempCode.Remove(i, 1);
                        CreatePicturebox("pbo_Check" + picCounter.ToString(), Color.White, 1200, 200, picCounter, "Die eingegebene Zahl hat den richtigen Wert und die falsche Position");
                        picCounter++;
                        i--;

                        break;
                    }
                }
            }
            return retryInput;
        }

        private void CreatePicturebox(string name, Color color, int locationX, int locationY, int picCounter, string message)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.BackColor = color;
            pictureBox.Size = new Size(30, 30);
            ToolTip toolTip = new ToolTip();
            pictureBox.Location = new Point(locationX + picCounter * 50, locationY + counter * 40);
            CreateTooltip(pictureBox, message);
            Game.Controls.Add(pictureBox);
        }

        private void EndGame(bool win)
        {
            var result = DialogResult.Cancel;
            if (win)
                result = MessageBox.Show("Du hast gewonnen!" + Environment.NewLine + "Der Code lautet: " + code[0], "Game Over", MessageBoxButtons.OK);
            else
                result = MessageBox.Show("Du hast verloren!", "Game Over", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
                Application.Exit();
        }

        private void CreateTooltip(Control Box, string message)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(Box, message);
        }

        private Button CreateButton(Button button, string text, int locationX, int locationY, int sizeX, int sizeY, Color backcolor, Font font, EventHandler click)
        {
            button.Text = text;
            button.Location = new Point(locationX, locationY);
            button.Size = new Size(sizeX, sizeY);
            button.BackColor = backcolor;
            button.Font = font;
            button.Click += click;
            Game.Controls.Add(button);
            return button;
        }

        private void CreateLabel(string name, string text, int locationX, int locationY, int sizeX, int sizeY, Color backcolor, Font font)
        {
            Label label = new Label();
            label.Name = name;
            label.Text = text;
            label.Location = new Point(locationX, locationY);
            label.Size = new Size(sizeX, sizeY);
            label.BackColor = backcolor;
            label.Font = font;
            Game.Controls.Add(label);
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;
            if (e.Index >= 0)
            {
                string n = ((ComboBox)sender).Items[e.Index].ToString();              
                Color c = Color.FromName(n);
                Brush b = new SolidBrush(c);
                g.DrawString(n, f, Brushes.Black, rect.X, rect.Top);
                g.FillRectangle(b, rect.X + 110, rect.Y + 5, rect.Width - 10, rect.Height - 10);
            }
        }
    }
}