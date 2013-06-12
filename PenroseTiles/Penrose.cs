using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PenroseTiles
{
    /// <summary>
    /// The penrose class. Create a seed of triangles to be subdivided.
    /// </summary>
    class Penrose
    {
        /// <summary>
        /// The list of triangles
        /// </summary>
        public List<Triangle> Triangles;

        /// <summary>
        /// Constructor create a list a 10 trinagles
        /// </summary>
        /// <param name="seed">Number of triangles in a wheel</param>
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

        /// <summary>
        /// Generate the penrose tiles
        /// </summary>
        /// <param name="genCount">Generation count</param>
        public void Generate(int genCount)
        {
            Parallel.ForEach(Triangles, T =>
            {
                T.SubDivide(genCount);
            });
        }
    }
}
