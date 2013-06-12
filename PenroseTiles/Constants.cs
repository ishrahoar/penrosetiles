using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenroseTiles
{
    class Constants
    {
        public static double GoldenRatio = (1 + Math.Sqrt(5)) / 2;

        public static Color SetSourceRGB(double r, double g, double b)
        {
            return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }
}
