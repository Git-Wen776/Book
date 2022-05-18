using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.YGCode
{
    public class CodeOpen
    {
        public readonly string CodeNum = "1234567890";
        public readonly string Encode = "ABCDEFGHIJKLOVERTYUQOPZXMNabcdefghijklovnnyutrqwzx";

        public int Codes { get; set; } = 4;
        public int Imgheight { get; set; } = 40;
        public int Imgwidth { get; set; } = 100;
        public int Ponits { get; set; } = 45;

        public Color[] Colors { get; set; } = {Color.DarkBlue, Color.Black,Color.Orange,Color.Red,Color.Purple,Color.YellowGreen};

    }
}
