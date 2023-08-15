using Stein_Schere_Shotgun.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stein_Schere_Shotgun
{
    public partial class app_SSS : Form
    {
        private Random rand = new Random();
        private int[] score = { 0, 0 };
        private int counter = 0;
        private Form Game;
        private Label lbl_header = new Label();
        private Label lbl_Computer = new Label();
        private Label lbl_Player = new Label();
        private Button btn_Stone = new Button();
        private Button btn_Scissor = new Button();
        private Button btn_Paper = new Button();
        private PictureBox pbo_Computer = new PictureBox();
        private PictureBox pbo_Player = new PictureBox();

        public app_SSS()
        {
            InitializeComponent();
        }

        public void Run(Form Game, Label lbl_header)
        {
            this.lbl_header = lbl_header;
            this.Game = Game;
            CreateUI();
        }

        public void CreateUI()
        {
            lbl_header.Text = "Rock Paper Scissors";
            Game.Controls.Add(lbl_header);
            lbl_Computer = CreateLabel(lbl_Computer, "Computer = 0", 200, 150, 400, 50, Color.LightGray, new Font("Broadway", 30));
            lbl_Player = CreateLabel(lbl_Player, "Computer = 0", 1000, 150, 400, 50, Color.LightGray, new Font("Broadway", 30));
            pbo_Computer = CreatePictureBox(pbo_Computer, 200, 250, 400, 400);
            pbo_Player = CreatePictureBox(pbo_Player, 1000, 250, 400, 400);
            btn_Stone = CreateButton(btn_Stone, "Stone", 200, 700, 300, 100, Color.LightGray, new Font("Broadway", 30), btn_Stone_Click);
            btn_Scissor = CreateButton(btn_Scissor, "Scissor", 650, 700, 300, 100, Color.LightGray, new Font("Broadway", 30), btn_Scissor_Click);
            btn_Paper = CreateButton(btn_Paper, "Paper", 1100, 700, 300, 100, Color.LightGray, new Font("Broadway", 30), btn_Paper_Click);
        }

        private Label CreateLabel(Label label, string text, int locationX, int locationY, int sizeX, int sizeY, Color backcolor, Font font)
        {
            label.Text = text;
            label.Location = new Point(locationX, locationY);
            label.Size = new Size(sizeX, sizeY);
            label.BackColor = backcolor;
            label.Font = font;
            Game.Controls.Add(label);
            return label;
        }

        private PictureBox CreatePictureBox(PictureBox pictureBox, int locationX, int locationY, int sizeX, int sizeY)
        {            
            pictureBox.Location = new Point(locationX, locationY);
            pictureBox.Size = new Size(sizeX, sizeY);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.BackColor = Color.White;
            Game.Controls.Add(pictureBox);
            return pictureBox;
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

        private void btn_Stone_Click(object sender, EventArgs e)
        {
            Compare(1, Resources.stone);
        }

        private void btn_Scissor_Click(object sender, EventArgs e)
        {
            Compare(2, Resources.scissor);
        }

        private void btn_Paper_Click(object sender, EventArgs e)
        {

            Compare(3, Resources.paper);
        }

        private void Compare(int player, Image image)
        {
            int computer = rand.Next(1, 4);
            pbo_Player.Image = image;
            switch (computer)
            {
                case 1:
                    pbo_Computer.Image = Resources.stone;
                    switch (player)
                    {
                        case 2:
                            score[0]++;
                            break;
                        case 3:
                            score[1]++;
                            break;
                    }
                    break;

                case 2:
                    pbo_Computer.Image = Resources.scissor;
                    switch (player)
                    {
                        case 1:
                            score[1]++;
                            break;
                        case 3:
                            score[0]++;
                            break;
                    }
                    break;

                case 3:
                    pbo_Computer.Image = Resources.paper;
                    switch (player)
                    {
                        case 1:
                            score[0]++;
                            break;
                        case 2:
                            score[1]++;
                            break;
                    }
                    break;
            }
            UpdateScore();
            counter++;
            if (counter >= 10)
            {
                var result = DialogResult.No;
                if (score[0] > score[1]) { result = MessageBox.Show("Computer wins", "Retry?", MessageBoxButtons.YesNo); }
                else if (score[0] < score[1]) { result = MessageBox.Show("Player wins", "Retry?", MessageBoxButtons.YesNo); }
                else { result = MessageBox.Show("Unentschieden", "Retry?", MessageBoxButtons.YesNo); }
                if (result == DialogResult.Yes) { score[0] = 0; score[1] = 0; counter = 0; UpdateScore(); }
                else { Application.Exit(); }
            }
        }
        public void UpdateScore()
        {
            lbl_Computer.Text = "Computer: " + score[0].ToString();
            lbl_Player.Text = "Player: " + score[1].ToString();
        }
    }
}