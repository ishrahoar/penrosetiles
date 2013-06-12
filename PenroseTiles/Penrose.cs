using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PenroseTiles
{
    class Penrose
    {
        public List<Triangle> Triangles;

        public Penrose(int seed = 10)
        {
            Triangles = new List<Triangle>();
            for (int i = 1; i <= seed; i++)
            {
                Complex b = Complex.FromPolarCoordinates(1, (((2 * i) - 1) * Math.PI) / seed);
                Complex c = Complex.FromPolarCoordinates(1, (((2 * i) + 1) * Math.PI) / seed);
                if (i % 2 == 0)
                {
                    Complex t;
                    t = b;
                    b = c;
                    c = t;
                }
                Triangles.Add(new Triangle(0, 0, b, c, 1));
            }
        }

        public void Generate(int genCount)
        {
            //foreach (Triangle2 T in Triangles)
            //{
            //    T.SubDivide(depth);
            //}
            Parallel.ForEach(Triangles, T =>
            {
                T.SubDivide(genCount);
            });
        }
    }
}
