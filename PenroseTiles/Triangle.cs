//New BSD License (BSD)
//
//Copyright (c) 2013, Harish K. Rao
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
//following conditions are met:
//
//* Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//disclaimer.
//
//* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//following disclaimer in the documentation and/or other materials provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
//OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
//AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
//USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
                Pen blackpen, colorpen;
                SolidBrush brush;
                PointF[] abc;
                Color color;

                // convert triangle in world co-ordinates to device co-ordinates
                abc = pa.ToDeviceCoOrd(this);
                
                color = (Colour == 0) ? Constants.SetSourceRGB(1.0, 0.35, 0.35) : Constants.SetSourceRGB(0.4, 0.4, 1.0);

                // color outlines
                colorpen = new Pen(color, 1);
                colorpen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawLine(colorpen, abc[0], abc[1]);
                g.DrawLine(colorpen, abc[1], abc[2]);
                colorpen.Dispose();

                // fill triangle
                brush = new SolidBrush(color);
                g.FillPolygon(brush, abc);
                brush.Dispose();

                // black outlines
                blackpen = new Pen(Constants.SetSourceRGB(0.2, 0.2, 0.2), 1);
                blackpen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawLine(blackpen, abc[2], abc[0]);
                g.DrawLine(blackpen, abc[0], abc[1]);
                blackpen.Dispose();
            }

            // Draw in parallel (NOT). Sleep(1) to yeild.
            Parallel.ForEach(SubDivisions, t =>
            {
                t.Draw(g, pa);
                System.Threading.Thread.Sleep(100);
            });
        }
    }
}
