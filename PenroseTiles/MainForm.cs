using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenroseTiles
{
    public partial class MainForm : Form
    {
        PlotArea plot;
        Penrose tiles;

        public MainForm()
        {
            InitializeComponent();
            plot = new PlotArea(1f, -1f, -1f, 1f, 3);
            this.BackColor = Color.White;
            tiles = new Penrose();
            tiles.Generate(8);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            plot.Width = ClientRectangle.Width - (2 * plot.Offset);
            plot.Height = ClientRectangle.Height - (2 * plot.Offset);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Parallel.ForEach(tiles.Triangles, T =>
            {
                T.Draw(g, plot);
            });
            g.Dispose();
        }
    }
}
