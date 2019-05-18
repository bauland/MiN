using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Min
{
    public class Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color() : this(0, 0, 0) { }
    }

}
