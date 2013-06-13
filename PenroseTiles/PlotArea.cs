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
using System.Drawing;
using System.Numerics;

namespace PenroseTiles
{
    /// <summary>
    /// Drawing Area defined in world co-ordinates.
    /// </summary>
    class PlotArea
    {
        private float MinXWorld { get; set; }
        private float MaxXWorld { get; set; }
        private float MinYWorld { get; set; }
        private float MaxYWorld { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="offset"></param>
        public PlotArea(float minX, float maxX, float minY, float maxY)
        {
            MinXWorld = minX;
            MaxXWorld = maxX;
            MinYWorld = minY;
            MaxYWorld = maxY;
            Height = 0;
            Width = 0;
        }

        /// <summary>
        /// Get 2D point in plot area in device co-ordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PointF ToDeviceCoOrd(float x, float y)
        {
            PointF p = new PointF();
            p.X = ((x - MinXWorld) * Width) / (MaxXWorld - MinXWorld);
            p.Y = Height - ((y - MinYWorld) * Height / (MaxYWorld - MinYWorld));
            return p;
        }

        /// <summary>
        /// Get 2D point in plot area in device co-ordinates
        /// </summary>
        /// <param name="point">Point structure</param>
        /// <returns></returns>
        public PointF ToDeviceCoOrd(PointF point)
        {
            PointF p;
            p = ToDeviceCoOrd(point.X, point.Y);
            return p;
        }

        /// <summary>
        /// Get 2D point in plot area in device co-ordinates
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public PointF ToDeviceCoOrd(Complex c)
        {
            PointF p;
            p = ToDeviceCoOrd((float)c.Real, (float)c.Imaginary);
            return p;
        }

        /// <summary>
        /// Convert triangle points in plot area in device co-ordinates
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public PointF[] ToDeviceCoOrd(Triangle t)
        {
            PointF[] abc = new PointF[3];
            abc[0] = ToDeviceCoOrd(t.A);
            abc[1] = ToDeviceCoOrd(t.B);
            abc[2] = ToDeviceCoOrd(t.C);
            return abc;
        }
    }
}
