using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace Sudoku
{
    public partial class app_sudoku : Form
    {
        private int[][] startValues =
        {
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            new int[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
            new int[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
            new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
            new int[] { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
            new int[] { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
            new int[] { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
            new int[] { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
            new int[] { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
        };

        private int[][] currentValues =
        {
            new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            new int[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
            new int[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
            new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
            new int[] { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
            new int[] { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
            new int[] { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
            new int[] { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
            new int[] { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
        };

        private int removeCounter = 0;
        private Form Game = new Form();
        private Label lbl_header = new Label();
        private List<GridButtonCustom> buttons = new List<GridButtonCustom>();
        private List<GridButtonCustom> buttons_empty = new List<GridButtonCustom>();
        private int[] pointer = new int[4];
        private int value = 0;
        private Random random = new Random();

        private GridButtonCustom btn_start = new GridButtonCustom();
        private List<RadioButtonCustom> radioButtons = new List<RadioButtonCustom>();

        public app_sudoku()
        {
            InitializeComponent();
        }

        public void Run(Form Game, Label lbl_header)
        {
            this.lbl_header = lbl_header;
            this.Game = Game;
            CreateUI();

        }

        private void CreateUI()
        {
            string radioText = "";
            int value;
            Game.Controls.Add(lbl_header);
            CreateCustomButton(null, "btn_start", new Point(1175, 150), new Size(200, 50), Color.White, new Font("Arial", 15), btn_Start_Click, -1, null);

            for (int panelY = 0; panelY < 3; panelY++)
            {
                if (panelY == 0)
                {
                    value = 40;
                    radioText = "Leicht";
                }
                else if (panelY == 1)
                {
                    value = 50;
                    radioText = "Medium";
                }
                else
                {
                    value = 55;
                    radioText = "Schwer";
                }
                CreateRadioButton("rdb_" + panelY.ToString(), radioText, new Point(1175, 275 + panelY * 150), new Size(200, 50), Color.White, new Font("Arial", 15), value, rdb_Click);
                for (int panelX = 0; panelX < 3; panelX++)
                {
                    CreateCustomButton(null, "btn_input" + panelY.ToString(), new Point(725 + panelX * 150, 250 + panelY * 150), new Size(100, 100), Color.Turquoise, new Font("Arial", 20), btn_Input_Click, panelY * 3 + panelX + 1, null);

                    Panel panel = new Panel();
                    panel = CreatePanel(panel, "pnl_" + panelY.ToString(), new Point(225 + panelX * 150, 225 + panelY * 150), new Size(150, 150));
                    panel.ForeColor = Color.Transparent; panel.BackColor = Color.Transparent;
                    Game.Controls.Add(panel);

                    for (int buttonY = 0; buttonY < 3; buttonY++)
                    {
                        pointer[1] = buttonY;
                        for (int buttonX = 0; buttonX < 3; buttonX++)
                        {
                            pointer[0] = buttonX + panelX * 3;
                            pointer[1] = buttonY + panelY * 3;
                            pointer[2] = panelX + panelY * 3;
                            CreateCustomButton(panel, "btn_" + buttonY.ToString() + buttonX.ToString(), new Point(0 + buttonX * 50, 0 + buttonY * 50), new Size(50, 50), Color.LightGray, new Font("Arial", 15), btn_Grid_Click, 0,
                                pointer);
                        }
                    }
                }
            }
        }

        private void InitializeArray()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    currentValues[i][j] = startValues[i][j];
                }
            }
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            CreateNumbers();
        }

        private void CreateNumbers()
        {
            InitializeArray();
            ShuffleNumbers();
            Shuffle();
            RemoveNumbers();
            PopulateGrid();
            Calculate();
            foreach (GridButtonCustom button in buttons_empty)
            {
                button.value = 0;
            }
        }

        private void Shuffle()
        {
            int branch;
            int section;
            int target;

            for (int i = 0; i < 100; i++)
            {
                branch = random.Next(2);
                section = random.Next(9);
                target = random.Next(1, 3);

                if (section % 3 == 0)
                {
                    target = section + target;
                }
                else if (section % 3 == 1)
                {
                    if (target == 1)
                        target = section - 1;
                    else
                        target = section + 1;
                }
                else
                {
                    target = section - target;
                }

                switch (branch)
                {
                    case 0:
                        ShuffleColumn(section, target);
                        break;

                    case 1:
                        ShuffleRow(section, target);
                        break;
                }
            }
        }

        private void rdb_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = true;
            removeCounter = ((RadioButtonCustom)sender).value;
        }

        private void ShuffleColumn(int section, int target)
        {
            int temp;
            for (int i = 0; i < 9; i++)
            {
                temp = currentValues[i][target];
                currentValues[i][target] = currentValues[i][section];
                currentValues[i][section] = temp;
            }
        }

        private void ShuffleRow(int section, int target)
        {
            int[] temp = new int[9];
            temp = currentValues[target];
            currentValues[target] = currentValues[section];
            currentValues[section] = temp;
        }

        private void ShuffleNumbers()
        {
            string numberValues = "123456789";
            string tempNumber = "";
            int[] numbersShuffled = new int[9];
            int numberPointer = 0;
            for (int i = 0; i < 9; i++)
            {
                numberPointer = random.Next(numberValues.Length);
                tempNumber = numberValues[numberPointer].ToString();
                numbersShuffled[i] = Convert.ToInt32(tempNumber);
                numberValues = numberValues.Remove(numberPointer, 1);
            }
            foreach (int[] ints in currentValues)
            {
                for (int i = 0; i < 9; i++)
                {
                    switch (ints[i])
                    {
                        case 1:
                            ints[i] = Convert.ToInt32(numbersShuffled[0]);
                            break;

                        case 2:
                            ints[i] = Convert.ToInt32(numbersShuffled[1]);
                            break;

                        case 3:
                            ints[i] = Convert.ToInt32(numbersShuffled[2]);
                            break;

                        case 4:
                            ints[i] = Convert.ToInt32(numbersShuffled[3]);
                            break;

                        case 5:
                            ints[i] = Convert.ToInt32(numbersShuffled[4]);
                            break;

                        case 6:
                            ints[i] = Convert.ToInt32(numbersShuffled[5]);
                            break;

                        case 7:
                            ints[i] = Convert.ToInt32(numbersShuffled[6]);
                            break;

                        case 8:
                            ints[i] = Convert.ToInt32(numbersShuffled[7]);
                            break;

                        case 9:
                            ints[i] = Convert.ToInt32(numbersShuffled[8]);
                            break;
                    }
                }
            }
        }

        private void CreateRadioButton(string name, string Text, Point location, Size size, Color color, Font font, int value, EventHandler click)
        {
            RadioButtonCustom radio = new RadioButtonCustom();

            radio.Name = name;
            radio.Text = Text;
            radio.Location = location;
            radio.Size = size;
            radio.BackColor = color;
            radio.Font = font;
            radio.value = value;
            radio.Click += click;
            radioButtons.Add(radio);
            Game.Controls.Add(radio);
        }

        private Panel CreatePanel(Panel panel, string name, Point location, Size size)
        {
            panel.Name = name;
            panel.Location = location;
            panel.Size = size;
            panel.BorderStyle = BorderStyle.FixedSingle;
            return panel;
        }

        private void CreateCustomButton(Panel panel, string name, Point location, Size size, Color color, Font font, EventHandler click, int number, int[] pointer)
        {
            GridButtonCustom buttonCustom = new GridButtonCustom();
            buttonCustom.Name = name;
            buttonCustom.Location = location;
            buttonCustom.Size = size;
            buttonCustom.BackColor = color;
            buttonCustom.Font = font;
            buttonCustom.TextAlign = ContentAlignment.MiddleCenter;
            buttonCustom.Click += click;

            if (number < 0)
            {
                buttonCustom.Enabled = false;
                buttonCustom.value = number;
                buttonCustom.Text = "Start";
                btn_start = buttonCustom;
                Game.Controls.Add(buttonCustom);
            }
            else
            {
                if (panel == null)
                {
                    buttonCustom.value = number;
                    buttonCustom.Text = buttonCustom.value.ToString();
                    Game.Controls.Add(buttonCustom);
                }
                else
                {
                    buttonCustom.gridLocation[0] = pointer[0];
                    buttonCustom.gridLocation[1] = pointer[1];
                    buttonCustom.gridLocation[2] = pointer[2];
                    buttonCustom.Enabled = false;
                    panel.Controls.Add(buttonCustom);
                    buttons.Add(buttonCustom);
                }
            }
        }

        private void RemoveNumbers()
        {
            int pointer1;
            int pointer2;
            for (int i = 0; i < removeCounter; i++)
            {
                pointer1 = random.Next(9);
                pointer2 = random.Next(9);
                if (currentValues[pointer1][pointer2] != 0)
                {
                    currentValues[pointer1][pointer2] = 0;
                }
                else
                {
                    i--;
                }
            }
        }

        private void PopulateGrid()
        {
            int counterRow = 0;
            int counterColumn = 0;
            int counter1 = 0;
            int counter2 = 0;
            int tempcolumn = 0;
            int tempRow = 0;
            buttons_empty.Clear();
            foreach (GridButtonCustom g in buttons)
            {
                g.Enabled = false;
                g.value = currentValues[counterRow][counterColumn];
                if (g.value == 0)
                {
                    g.Text = "";
                    buttons_empty.Add(g);
                }
                else
                    g.Text = g.value.ToString();

                counter1++;
                counterColumn++;

                if (counter1 % 3 == 0)
                {
                    counterColumn = tempcolumn;
                    counterRow++;
                    if (counter1 % 9 == 0)
                    {
                        tempcolumn += 3;
                        counterColumn = tempcolumn;
                        counterRow = tempRow;
                        counter1 = 0;
                        counter2++;
                        if (counter2 % 3 == 0)
                        {
                            tempRow += 3;
                            counterRow = tempRow;
                            counter2 = 0;
                            tempcolumn = 0;
                            counterColumn = tempcolumn;
                        }
                    }
                }
            }
        }

        private void btn_Grid_Click(object sender, EventArgs e)
        {
            GridButtonCustom clickedButton = (GridButtonCustom)sender;
            clickedButton.value = value;
            clickedButton.Text = value.ToString();
            foreach (GridButtonCustom button in buttons)
            {
                button.Enabled = false;
            }
            EndGame();
        }

        private void btn_Input_Click(object sender, EventArgs e)
        {
            GridButtonCustom clickedButton = (GridButtonCustom)sender;
            value = clickedButton.value;
            foreach (GridButtonCustom button in buttons_empty)
            {
                if (button.BackColor == Color.Red)
                {
                    button.value = 0;
                    button.Text = string.Empty;
                    button.BackColor = Color.LightGray;
                }
                button.Enabled = true;
            }
        }

        private void Calculate()
        {
            int counterValid;
            int counterSolved = 0;
            int counterLoop = 0;
            bool valid;
            bool boardFull;
            int number;

            do
            {
                boardFull = true;
                counterLoop++;
                foreach (GridButtonCustom button in buttons_empty)
                {
                    counterValid = 0;
                    number = 0;
                    for (int j = 1; j < 10; j++)
                    {
                        valid = true;
                        foreach (GridButtonCustom checkButton in buttons)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (button.gridLocation[i] == checkButton.gridLocation[i] && checkButton.value == j)
                                {
                                    valid = false;
                                    break;
                                }
                            }
                            if (!valid)
                                break;
                        }
                        if (valid)
                        {
                            number = j;
                            counterValid++;
                        }
                    }
                    if (counterValid == 1)
                    {
                        counterSolved++;
                        button.value = number;
                    }
                }
                foreach (GridButtonCustom button in buttons_empty)
                {
                    if (button.value == 0)
                    {
                        boardFull = false;
                    }
                }
            } while (!boardFull && counterSolved != removeCounter && counterLoop < 10);
            if (!boardFull)
            {
                CreateNumbers();
            }
        }

        private void EndGame()
        {
            bool endgame = true;
            foreach (GridButtonCustom button in buttons)
            {
                if (button.value == 0)
                {
                    endgame = false;
                }
            }
            if (endgame)
            {
                var result = true;
                foreach (GridButtonCustom button in buttons_empty)
                {
                    foreach (GridButtonCustom button2 in buttons)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (button.gridLocation[i] == button2.gridLocation[i] && button.value == button2.value && button != button2)
                            {
                                button.BackColor = Color.Red;
                                result = false;
                                break;
                            }
                        }
                        if (button.BackColor == Color.Red)
                            break;
                    }
                }
                if (result)
                    MessageBox.Show("Du hast gewonnen", "Game Over", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Du hast verloren", "Game Over", MessageBoxButtons.OK);
            }
        }

    }
}