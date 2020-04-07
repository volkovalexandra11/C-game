using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class GameForm : Form
    {
        private GameState gs { get; set; }
        private readonly Timer timer;

        protected override void OnPaint(PaintEventArgs e)
        {
            var horizontalScalar = ClientSize.Width / 1280f;
            var verticalScalar = ClientSize.Height / 720f; //TODO; 1280*720 ?
            e.Graphics.ScaleTransform(horizontalScalar, verticalScalar);
            DrawNormalizedForm(e.Graphics);
        }

        public void DrawNormalizedForm(Graphics g)
        {
            g.FillRectangle(Brushes.Green,
                gs.Player.Pos.X - Player.Width / 2f, gs.Player.Pos.Y - Player.Height,
                Player.Width, Player.Height);
        }

        public GameForm(GameState gs)
        {
            this.gs = gs;
            DoubleBuffered = true;
            timer = new Timer() { Interval = (int)(gs.Dt / 1000) };
            KeyDown += (sender, args) =>
            {
                gs.Player.UpdateState(args);
            };
            SizeChanged += (sender, args) => { Invalidate(); };
            timer.Tick += (sender, args) =>
            {
                gs.Player.UpdatePosition();
                Invalidate();
            };
            timer.Start();
        }
    }
}
