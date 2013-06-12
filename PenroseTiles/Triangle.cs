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
        public int TriColor { get; set; }
        public Complex A { get; set; }
        public Complex B { get; set; }
        public Complex C { get; set; }
        public List<Triangle> Children;
        private bool IsDivided { get; set; }
        private int Level { get; set; }

        public Triangle(int color, Complex a, Complex b, Complex c, int level)
        {
            TriColor = color;
            A = a;
            B = b;
            C = c;
            IsDivided = false;
            Children = new List<Triangle>();
            Level = level;
        }

        public void SubDivide(int depth)
        {
            if (!IsDivided && Level != depth)
            {
                Complex p;
                Complex q;
                Complex r;
                if (TriColor == 0)
                {
                    p = A + ((B - A) / Constants.GoldenRatio);
                    Children.Add(new Triangle(0, C, p, B, Level + 1));
                    Children.Add(new Triangle(1, p, C, A, Level + 1));
                }
                else
                {
                    q = B + ((A - B) / Constants.GoldenRatio);
                    r = B + ((C - B) / Constants.GoldenRatio);
                    Children.Add(new Triangle(1, r, C, A, Level + 1));
                    Children.Add(new Triangle(1, q, r, B, Level + 1));
                    Children.Add(new Triangle(0, r, q, A, Level + 1));
                }
                
                //foreach (Triangle abc in Children)
                //{
                //    abc.SubDivide(depth);
                //}

                Parallel.ForEach(Children, c =>
                {
                    c.SubDivide(depth);
                });
            }
        }

        public void Draw(Graphics g, PlotArea pa)
        {
            lock (g)
            {
                Pen blackpen = new Pen(Color.Black, 1);
                blackpen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                SolidBrush colorbrush;
                PointF[] abc;
                abc = pa.ToDeviceCoOrd(this);
                if (TriColor == 0)
                {
                    colorbrush = new SolidBrush(Constants.SetSourceRGB(1.0, 0.35, 0.35));
                }
                else
                {
                    colorbrush = new SolidBrush(Constants.SetSourceRGB(0.4, 0.4, 1.0));
                }
                g.FillPolygon(colorbrush, abc);
                g.DrawLine(blackpen, abc[2], abc[0]);
                g.DrawLine(blackpen, abc[0], abc[1]);
                blackpen.Dispose();
                colorbrush.Dispose();
            }
            //foreach (Triangle t in Children)
            //{
            //    t.Draw(g, pa);
            //    System.Threading.Thread.Sleep(1);
            //}
            Parallel.ForEach(Children, c =>
            {
                c.Draw(g, pa);
                System.Threading.Thread.Sleep(1);
            });
        }
    }
}
