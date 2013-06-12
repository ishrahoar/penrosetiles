using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace PenroseTiles
{
    class Triangle
    {
        private int Colour { get; set; }
        public Complex A { get; set; }
        public Complex B { get; set; }
        public Complex C { get; set; }
        private List<Triangle> SubDivisions;
        private int Generation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">Color Red (0) or Blue (1)</param>
        /// <param name="a">Triangle vertex A</param>
        /// <param name="b">Triangle vertex B</param>
        /// <param name="c">Triangle vertex C</param>
        /// <param name="gen">Generation# of the triangle</param>
        public Triangle(int color, Complex a, Complex b, Complex c, int gen)
        {
            Colour = color;
            A = a;
            B = b;
            C = c;
            SubDivisions = new List<Triangle>();
            Generation = gen;
        }

        /// <summary>
        /// Recursively sub-divides triangles until generation limit.
        /// </summary>
        /// <param name="genLimit">Number of genertions</param>
        public void SubDivide(int genLimit)
        {
            if (SubDivisions.Count == 0 && Generation != genLimit)
            {
                Complex p;
                Complex q;
                Complex r;
                if (Colour == 0)
                {
                    // divide red triangle into a red & blue triangle
                    p = A + ((B - A) / Constants.GoldenRatio);
                    SubDivisions.Add(new Triangle(0, C, p, B, Generation + 1));
                    SubDivisions.Add(new Triangle(1, p, C, A, Generation + 1));
                }
                else
                {
                    // divide blue triangle into two red triangles & one blue triangle
                    q = B + ((A - B) / Constants.GoldenRatio);
                    r = B + ((C - B) / Constants.GoldenRatio);
                    SubDivisions.Add(new Triangle(1, r, C, A, Generation + 1));
                    SubDivisions.Add(new Triangle(1, q, r, B, Generation + 1));
                    SubDivisions.Add(new Triangle(0, r, q, A, Generation + 1));
                }
                // sub divide in parallel
                Parallel.ForEach(SubDivisions, t =>
                {
                    t.SubDivide(genLimit);
                });
            }
        }

        /// <summary>
        /// Draw the triangles & outlines.
        /// </summary>
        /// <param name="g">GDI+ graphics object</param>
        /// <param name="pa">Plot Area</param>
        public void Draw(Graphics g, PlotArea pa)
        {
            lock (g) // GDI+ graphics is not threadsafe
            {
                Pen blackpen = new Pen(Constants.SetSourceRGB(0.2, 0.2, 0.2), 1);
                Pen colorpen;
                blackpen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                SolidBrush brush;
                PointF[] abc;

                // convert triangle in world co-ordinates to device co-ordinates
                abc = pa.ToDeviceCoOrd(this);

                if (Colour == 0)
                {
                    Color color = Constants.SetSourceRGB(1.0, 0.35, 0.35);
                    brush = new SolidBrush(color);
                    colorpen = new Pen(color, 1);
                }
                else
                {
                    Color color = Constants.SetSourceRGB(0.4, 0.4, 1.0);
                    brush = new SolidBrush(color);
                    colorpen = new Pen(color, 1);
                }

                // color outlines
                colorpen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawLine(colorpen, abc[0], abc[1]);
                g.DrawLine(colorpen, abc[1], abc[2]);
                
                // fill triangle
                g.FillPolygon(brush, abc);

                // black outlines
                g.DrawLine(blackpen, abc[2], abc[0]);
                g.DrawLine(blackpen, abc[0], abc[1]);
                blackpen.Dispose();
                brush.Dispose();
            }

            // Draw in parallel. Sleep(1) to yeild.
            Parallel.ForEach(SubDivisions, t =>
            {
                t.Draw(g, pa);
                System.Threading.Thread.Sleep(1);
            });
        }
    }
}
