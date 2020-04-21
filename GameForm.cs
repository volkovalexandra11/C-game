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
        private GameState game { get; set; }
        private readonly Timer timer;
        public Size InternalSize { get; }

        protected override void OnPaint(PaintEventArgs e)
        {
            var horizontalScalar = ClientSize.Width / (float)InternalSize.Width;
            var verticalScalar = ClientSize.Height / (float)InternalSize.Height; //BASIC internal coords 1800x1000
            var minScalar = Math.Min(horizontalScalar, verticalScalar);
            e.Graphics.ScaleTransform(minScalar, minScalar);
            if (verticalScalar <= horizontalScalar)
                e.Graphics.TranslateTransform((ClientSize.Width/verticalScalar - InternalSize.Width)/2, 0);
            else
                e.Graphics.TranslateTransform(0, (ClientSize.Height/horizontalScalar - InternalSize.Height)/2);
            DrawNormalizedForm(e.Graphics);
        }

        private void DrawNormalizedForm(Graphics g)
        {
            game.Level.Draw(g);
        }

        public GameForm(GameState gs)
        {
            ClientSize = new Size(1260, 700);
            game = gs;
            DoubleBuffered = true;
            BackColor = Color.Black;
            InternalSize = new Size(1800, 1000);
            SizeChanged += (sender, args) => { Invalidate(); };
            timer = new Timer() { Interval = gs.Dt };
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        protected void OnTimerTick(object sender, EventArgs args)
        {
            game.UpdateModel();
            Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            game.HandleKey(e.KeyCode, false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            game.HandleKey(e.KeyCode, true);
        }
    }
}
