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
        PlotArea plot; // 2D plot area
        Penrose tiles; // penrose tiles

        public MainForm()
        {
            InitializeComponent();
            
            // define the plot area
            plot = new PlotArea(-1f, 1f, -1f, 1f);

            this.BackColor = Color.White;
            tiles = new Penrose();
            tiles.Generate(8);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            int offset = 5;
            // set the plot area width/height
            plot.Width = ClientRectangle.Width - (2 * offset);
            plot.Height = ClientRectangle.Height - (2 * offset);

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
