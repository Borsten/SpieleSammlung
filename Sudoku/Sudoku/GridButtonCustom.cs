using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http.Headers;

namespace Sudoku
{
    public class GridButtonCustom : Button
    {
        public int[] gridLocation = new int[3];

        public int value { get; set; }
    }
}

